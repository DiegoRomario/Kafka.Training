using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class ProducerService<T>
    {

        private readonly ProducerConfig _config;

        public ProducerService(ProducerConfig config)
        {
            _config = config;
        }
        public async Task<string> Run(string topic, T Tmessage)
        {

            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                try
                {
                    var message = new Message<string, string>() { Value = JsonSerializer.Serialize<T>(Tmessage), Key = Guid.NewGuid().ToString()};
                    DeliveryResult<string, string> demo = await producer.ProduceAsync(topic, message);
                    return $"Sent message: {demo.Message.Value} / to topic: {demo.Topic} / partition: {demo.Partition} / offset: {demo.Offset} ";
                }
                catch (ProduceException<Null, string>)
                {
                    throw;
                }

            }
        }
    }
}
