using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqProducer;

public static class Program
{
    private static readonly string queueName = "queue1";
    private static readonly string exchangeName = "exchange1";

    static void Main()
    {
        Console.WriteLine("Consumer started");

        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName = "rabbitmq";
        using IConnection connection = factory.CreateConnection();
        using IModel channel = connection.CreateModel();

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queueName, exchangeName, "", null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
        {
            var body = ea.Body.ToArray();
            //Console.WriteLine($"Received {body[0]}");
        };

        string consumerTag = channel.BasicConsume(queueName, autoAck: true, consumer);

        _ = Console.ReadLine();
    }
}
