using Grpc.Core;
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
            var chatService = new V1.Chat.ChatClient(channel);

            Console.WriteLine("Inserire il nome.");
            var user = Console.ReadLine();

            Console.WriteLine("Inserire i messaggi da inviare. Scrivere [exit] per uscire ...");


            // connect to gRPC service and open stream
            //
            using (var call = chatService.SendMessage())
            {
                // manage response stream
                //
                var responseTask = Task.Run(async () =>
                {
                    await foreach (var response in call.ResponseStream.ReadAllAsync())
                    {
                        Console.ForegroundColor = response.Status ? ConsoleColor.Green : ConsoleColor.Red;
                        Console.WriteLine($"Messaggio {response.MessageId} status {response.Status}");
                        Console.ResetColor();
                    }
                });

                while (true)
                {
                    // retrieve message to send
                    //
                    var message = Console.ReadLine();

                    // manage console exit
                    //
                    if (string.Compare(message, "exit", true) == 0)
                    {
                        await call.RequestStream.CompleteAsync();
                        await responseTask;
                        return;
                    }

                    try
                    {
                        // send message to gRPC service
                        //
                        await call.RequestStream.WriteAsync(new V1.SendMessageRequest
                        {
                            Id = Guid.NewGuid().ToString(),
                            From = user,
                            Message = message
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.ToString());
                        Console.ResetColor();
                    }
                }
            }
        }
    }
}
