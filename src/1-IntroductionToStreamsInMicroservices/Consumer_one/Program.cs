using Confluent.Kafka;
using Core;
using System;
using System.Threading;

namespace Consumer_one
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            var service = new ConsumerService();
            service.Consume("TOPPER", "NO", "IDNO1");

        }
    }
}
