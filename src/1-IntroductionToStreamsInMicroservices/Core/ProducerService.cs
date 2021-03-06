﻿using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core
{
    public class ProducerService<T>
    {

        public ProducerConfig _config { get; private set; }

        public ProducerService()
        {
            _config = new ProducerConfig();
            _config.BootstrapServers = "localhost:9092";
        }
        public async Task<string> Run(string topic, T Tmessage)
        {

            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                try
                {
                    var message = new Message<string, string>() { Value = JsonSerializer.Serialize<T>(Tmessage), Key = Guid.NewGuid().ToString()};
                    DeliveryResult<string, string> demo = await producer.ProduceAsync(topic, message);
                    return $"Sent to topic: {demo.Topic} / partition: {demo.Partition} / offset: {demo.Offset} ";
                }
                catch (ProduceException<Null, string>)
                {
                    throw;
                }

            }
        }
    }
}
