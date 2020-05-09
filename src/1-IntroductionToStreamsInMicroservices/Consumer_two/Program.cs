using Core;
using System;

namespace Consumer_two
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;


            var service = new ConsumerService();

            while (true)
            {
                Console.WriteLine(service.Consume("DEMO_ORDER", "Consumer_two", "Im Consumer two"));
            }

        }
    }
}
