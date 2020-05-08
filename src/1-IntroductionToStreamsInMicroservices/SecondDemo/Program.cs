using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SecondDemo
{
    class Program
    {
        static void Main(string[] args)
        {


            var config = new ConsumerConfig()
            {
                GroupId = "12",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {

                consumer.Subscribe("demotopic");

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
