using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


/// <summary>
///     An example of working with JSON data, Apache Kafka and 
///     Confluent Schema Registry (v5.5 or later required for
///     JSON schema support).
/// </summary>
namespace JsonSerialization
{
    /// <summary>
    ///     A POCO class corresponding to the JSON data written
    ///     to Kafka, where the schema is implicitly defined through 
    ///     the class properties and their attributes.
    /// </summary>
    /// <remarks>
    ///     Internally, the JSON serializer uses Newtonsoft.Json for
    ///     serialization and NJsonSchema for schema creation and
    ///     validation. You can use any property annotations recognised
    ///     by these libraries.
    ///
    ///     Note: Off-the-shelf libraries do not yet exist to enable
    ///     integration of System.Text.Json and JSON Schema, so this
    ///     is not yet supported by the Confluent serializers.
    /// </remarks>
    class Person
    {
        [JsonRequired] // use Newtonsoft.Json annotations
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonRequired]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Range(0, 150)] // or System.ComponentModel.DataAnnotations annotations
        [JsonProperty("age")]
        public int Age { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {

            string bootstrapServers = "localhost:9092";
            string schemaRegistryUrl = "https://localhost:8082";
            string topicName = "DEMO_ORDER";

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                // Note: you can specify more than one schema registry url using the
                // schema.registry.url property for redundancy (comma separated list). 
                // The property name is not plural to follow the convention set by
                // the Java implementation.
                Url = schemaRegistryUrl
            };

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = "json-example-consumer-group"
            };

            // Note: Specifying json serializer configuration is optional.
            var jsonSerializerConfig = new JsonSerializerConfig
            {
                BufferBytes = 100
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            var consumeTask = Task.Run(() =>
            {
                using (var consumer =
                    new ConsumerBuilder<Null, Person>(consumerConfig)
                        .SetValueDeserializer(new JsonDeserializer<Person>().AsSyncOverAsync())
                        .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                        .Build())
                {
                    consumer.Subscribe(topicName);

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var cr = consumer.Consume(cts.Token);
                                Console.WriteLine($"Name: {cr.Message.Value.FirstName} {cr.Message.Value.LastName}, age: {cr.Message.Value.Age}");
                            }
                            catch (ConsumeException e)
                            {
                                Console.WriteLine($"Consume error: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                    }
                }
            });

            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var producer =
                new ProducerBuilder<Null, Person>(producerConfig)
                    .SetValueSerializer(new JsonSerializer<Person>(schemaRegistry, jsonSerializerConfig))
                    .Build())
            {
                Console.WriteLine($"{producer.Name} producing on {topicName}. Enter first names, q to exit.");

                int i = 0;
                string text;
                while ((text = Console.ReadLine()) != "q")
                {
                    Person person = new Person { FirstName = text, LastName = "lastname", Age = i++ % 150 };
                    DeliveryResult<Null, Person> demo = await producer
                        .ProduceAsync(topicName, new Message<Null, Person> { Value = person });
                        //.ContinueWith(task => task.IsFaulted
                        //    ? $"error producing message: {task.Exception.Message}"
                        //    : $"produced to: {task.Result.TopicPartitionOffset}");

                    Console.WriteLine(demo);
                }
            }

            cts.Cancel();

            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            {
                // Note: a subject name strategy was not configured, so the default "Topic" was used.
                var schema = await schemaRegistry.GetLatestSchemaAsync(SubjectNameStrategy.Topic.ConstructValueSubjectName(topicName));
                Console.WriteLine("\nThe JSON schema corresponding to the written data:");
                Console.WriteLine(schema.SchemaString);
            }
        }
    }
}