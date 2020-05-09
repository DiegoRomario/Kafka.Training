using Core;
using System;
using System.Threading.Tasks;

namespace Producer_one
{
    class Program
    {
        static async Task Main(string[] args)
        {

            ProducerService service = new ProducerService();
            while (true) {
                Console.WriteLine(await service.Run("DEMO_ORDER"));
            }

        }
    }
}
