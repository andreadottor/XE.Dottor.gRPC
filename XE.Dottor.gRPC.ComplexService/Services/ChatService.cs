using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XE.Dottor.gRPC.ComplexService.Hubs;

namespace XE.Dottor.gRPC.ComplexService.Services
{
    public class ChatService : V1.Chat.ChatBase
    {
        
        private readonly IHubContext<TextHub> _hubContext;
        private readonly ILogger<ChatService> _logger;
        public ChatService(ILogger<ChatService> logger, IHubContext<TextHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public override async Task SendMessage(IAsyncStreamReader<V1.SendMessageRequest> requestStream, IServerStreamWriter<V1.SendMessageReply> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                // Get the client message from the request stream
                var message = requestStream.Current;
                var response = new V1.SendMessageReply()
                {
                    MessageId = message.Id
                };

                try
                {
                    await _hubContext.Clients.All.SendAsync("AddText", message.From, message.Message);
                    response.Status = true;
                }
                catch (Exception)
                {
                    response.Status = false;
                }

                await responseStream.WriteAsync(response);
                
            }
        }
    }
}
