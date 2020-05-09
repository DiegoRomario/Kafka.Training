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
            service.Consume("TOPPER", "NO", "IDNO2");


        }
    }
}
