using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace XE.Dottor.gRPC.ComplexService.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            // create the client
            var chatService = new Chat.ChatClient(channel);

            Console.WriteLine("Inserire il nome.");
            var user = Console.ReadLine();

            Console.WriteLine("Inserire i messaggi da inviare. Scrivere [exit] per uscire ...");

            using (var call = chatService.SendMessage())
            {
                while(true)
                {
                    var message = Console.ReadLine();
                    if (string.Compare(message, "exit", true) == 0)
                        return;

                    try
                    {
                        await call.RequestStream.WriteAsync(new SendMessageRequest
                        {
                            From = user,
                            Message = message
                        });
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.ToString());
                    }
                } 
                 
            }
        }
    }
}
