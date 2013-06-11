using System;
using MassTransit;
using MassTransit.Subscriptions.Coordinator;
using Models;
using Storage;
using Subscriber.Consumers;

namespace Subscriber
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bus = ServiceBusFactory.New(sbc =>
                {
                    sbc.UseMsmq(m => {
                            m.VerifyMsmqConfiguration();
                            m.UseMulticastSubscriptionClient();
                        });
                    sbc.SetNetwork("myNetworkName");
                    sbc.ReceiveFrom("msmq://localhost/test_queue_subscriber");
                    sbc.UseSubscriptionStorage(SubscriptionStorageFactory);
                    sbc.Subscribe(s =>
                        {
                            s.Consumer<PublisherMessageConsumer>();
                            s.Consumer<TestMessageConsumer>();
                        });
                });

            //bus.SubscribeHandler(new Action<PublisherMessage>(OnPublisherMessageReceived));

            Console.WriteLine("Subscriber configured!");

            do
            {
                bus.Publish(new SubscriberMessage {Text = "Hi from Subscriber"});

                Console.ReadLine();
            } while (true);
        }

        private static SubscriptionStorage SubscriptionStorageFactory()
        {
            return new RavenSubscriptionStorage();
        }
    }
}