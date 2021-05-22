using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "taskQueu",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                
                channel.BasicQos(0,1,false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    Console.WriteLine($"Received: {message}");

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: "taskQueu",
                    autoAck: false,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}