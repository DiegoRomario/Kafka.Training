using Confluent.Kafka;
using System;
using System.Threading;

namespace Consumer_one
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            var group = typeof(Program).Assembly.GetName().Name;
            var config = new ConsumerConfig()
            {
                GroupId = group,
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest,
                ClientId = "I'M CONSUMER ONE"

            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {

                consumer.Subscribe("DEMO_ORDER");

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
                }

            }


        }
    }
}
