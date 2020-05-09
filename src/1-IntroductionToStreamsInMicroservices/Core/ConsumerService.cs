using Confluent.Kafka;
using System;
using System.Threading;

namespace Core
{
    public class ConsumerService
    {

        public string Consume(string topic, string group, string clientID)
        {

            var config = new ConsumerConfig()
            {
                GroupId = group,
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest,
                ClientId = clientID

            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {

                consumer.Subscribe(topic);

                var token = new CancellationTokenSource();

                try
                {
                    while (true)
                    {
                       ConsumeResult<string, string> message = consumer.Consume(token.Token);
                       Console.WriteLine($"Received [{message.Message.Value}] to topic: {message.Topic} / partition: {message.Partition} / offset: {message.Offset} /"); 
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                    throw;
                }

            }
        }
    }
}
