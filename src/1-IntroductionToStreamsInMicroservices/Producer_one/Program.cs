using Core;
using System;
using System.Threading.Tasks;

namespace Producer_one
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var service = new ProducerService<Order>();
            while (true) {
                Console.WriteLine(await service.Run("TOPPER", new Order() { Id = 666, Amount = 155.00m, Client = "Diego" }));
            }

        }
    }
}
