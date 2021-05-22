using System;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "taskQueu",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var messages = Messages.GetMessageArray();

                    for (int i = 0; i <= 12; i++)
                {
                    string message = messages[i];
                    var body = Encoding.UTF8.GetBytes(message);

                    var properities = channel.CreateBasicProperties();
                    properities.Persistent = true;

                    channel.BasicPublish(exchange: "",
                        routingKey: "taskQueu",
                        basicProperties: properities,
                        body: body);
                    Console.WriteLine($"Sent: {message}");    
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();  
        }
    }
}