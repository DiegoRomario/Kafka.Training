using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Producer_one
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
            };
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        Thread.Sleep(2500);
                        var message = new Message<string, string>() { Value = $"Message Id: {Guid.NewGuid().ToString() }", Key = Guid.NewGuid().ToString() };
                        DeliveryResult<string, string> demo = await producer.ProduceAsync("DEMO_ORDER", message);
                        Console.WriteLine($"Sent to topic: {demo.Topic} / partition: {demo.Partition} / offset: {demo.Offset} ");


                    }
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"The message failed: {e.Error.Reason}");
                }
                Console.ReadKey();

            }


        }
    }
}
