using System;
using MassTransit;
using Models;

namespace Subscriber.Consumers
{
    public class PublisherMessageConsumer : Consumes<PublisherMessage>.All
    {
        public void Consume(PublisherMessage message)
        {
            Console.WriteLine(message.Text);
        }
    }
}