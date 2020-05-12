using Confluent.Kafka;
using Core;
using System;
using System.Threading.Tasks;

namespace Producer_one
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092,localhost:9093,localhost:9094",
                Acks = Acks.All,
                MaxInFlight = 5

            };
            var service = new ProducerService<string>(config);
            for (int i = 0; i < 12000; i++)
            {
                Console.WriteLine(await service.Run("mytopic", i.ToString()));
            }

        }


    }


}
