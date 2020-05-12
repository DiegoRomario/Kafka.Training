using Core;
using System;

namespace Consumer_log
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            var service = new ConsumerService();
            service.Consume("mytopic", "GO", "GO");
        }
    }
}
