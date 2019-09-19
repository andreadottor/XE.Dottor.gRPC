using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace XE.Dottor.gRPC.SimpleService.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            //
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine();

            // Call XE.Events service
            //
            var clientXeEvents = new Events.EventsClient(channel);
            var eventsReply = await clientXeEvents.GetEventsAsync(new GetEventsRequest());

            foreach (var item in eventsReply.Events)
            {
                var date = DateTime.Parse(item.Date);
                Console.WriteLine($"{date.ToShortDateString()}: {item.Title}");

                foreach (var session in item.Sessions)
                {
                    Console.WriteLine($"\t{session.Author} - {session.Title}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
