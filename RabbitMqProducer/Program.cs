using RabbitMQ.Client;

namespace RabbitMqProducer;

public static class Program
{
    private static readonly string queueName = "queue1";
    private static readonly string exchangeName = "exchange1";

    static void Main()
    {
        Console.WriteLine("Producer started");

        ConnectionFactory factory = new ConnectionFactory();
        factory.HostName = "rabbitmq";
        using IConnection connection = factory.CreateConnection();
        using IModel channel = connection.CreateModel();

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queueName, exchangeName, "", null);

        IBasicProperties props = channel.CreateBasicProperties();
        props.DeliveryMode = 1;

        byte[] buffer = new byte[1];
        int counter = 0;

        while (counter > 0)
        {
            buffer[0] = (byte)counter++;
            channel.BasicPublish(exchangeName, "", props, buffer);
            Thread.Sleep(1000);
        }

        channel.Close();
        connection.Close();

        Console.WriteLine("Producer finished");
    }
}
