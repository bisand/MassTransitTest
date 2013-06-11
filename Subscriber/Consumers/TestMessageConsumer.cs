using System;
using MassTransit;
using Models;

namespace Subscriber.Consumers
{
    public class TestMessageConsumer : Consumes<TestMessage>.All
    {
        public void Consume(TestMessage message)
        {
            Console.WriteLine(message.Text);
        }
    }
}