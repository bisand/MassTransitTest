using System;
using MassTransit;
using MassTransit.Subscriptions.Coordinator;
using Models;
using Storage;

namespace Publisher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bus = ServiceBusFactory.New(sbc =>
                {
                    sbc.UseMsmq(m =>
                        {
                            m.VerifyMsmqConfiguration();
                            m.UseMulticastSubscriptionClient();
                        });
                    sbc.SetNetwork("myNetworkName");
                    sbc.ReceiveFrom("msmq://localhost/test_queue_publisher");
                    sbc.UseSubscriptionStorage(SubscriptionStorageFactory);
                    sbc.Subscribe(s => s.Handler<SubscriberMessage>(OnSubscriberMessageReceived));
                });

            //bus.SubscribeHandler(new Action<SubscriberMessage>(OnSubscriberMessageReceived));

            Console.WriteLine("Publisher configured!");

            do
            {
                bus.Publish(new PublisherMessage { Text = "Hi from Publisher" });
                bus.Publish(new TestMessage { Text = "This is a test message from publisher" });

                Console.ReadLine();
            } while (true);
        }

        private static SubscriptionStorage SubscriptionStorageFactory()
        {
            return new RavenSubscriptionStorage();
        }

        private static void OnSubscriberMessageReceived(SubscriberMessage msg)
        {
            Console.WriteLine(msg.Text);
        }
    }
}