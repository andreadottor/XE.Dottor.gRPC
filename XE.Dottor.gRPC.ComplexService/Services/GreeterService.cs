using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using XE.Dottor.gRPC.ComplexService.Hubs;

namespace XE.Dottor.gRPC.ComplexService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IHubContext<TextHub> _hubContext;
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger, IHubContext<TextHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            await _hubContext.Clients.All.SendAsync("AddText", request.Name);
            return await Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
