using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XE.Dottor.gRPC.SimpleService.Services
{
    public class EventsService : Events.EventsBase
    {
        private readonly ILogger<EventsService> _logger;
        public EventsService(ILogger<EventsService> logger)
        {
            _logger = logger;
        }


        public override Task<GetEventsReply> GetEvents(GetEventsRequest request, ServerCallContext context)
        {
            var response = new GetEventsReply();

            // gRPC and c# Optimising Night
            //
            var grpcEvent = new XeEvent()
            {
                Title = "gRPC and c# Optimising Night",
                Date = new DateTime(2019, 09, 20).ToString("s"),
            };
            grpcEvent.Sessions.Add(new XeEventSession
            {
                Title = "Alla scoperta di gRPC",
                Author = "Andrea Dottor"
            });
            grpcEvent.Sessions.Add(new XeEventSession
            {
                Title = "Optimising Code Using Span<T>",
                Author = "Mirco Vanini"
            });
            response.Events.Add(grpcEvent);

            // .NET Conf 2019 - guardiamo assieme lo streaming
            //
            var netConfEvent = new XeEvent()
            {
                Title = ".NET Conf 2019 - guardiamo assieme lo streaming",
                Date = new DateTime(2019, 09, 23).ToString("s"),
            };
            response.Events.Add(netConfEvent);

            // Team working in action
            //
            var teamWorkingEvent = new XeEvent()
            {
                Title = "Team working in action",
                Date = new DateTime(2019, 10, 18).ToString("s"),
            };
            teamWorkingEvent.Sessions.Add(new XeEventSession
            {
                Title = "Team working in action",
                Author = "Enrico Illuminati"
            });
            response.Events.Add(teamWorkingEvent);

            // Visual Studio Saturday 2019
            //
            var vsSaturdayEvent = new XeEvent()
            {
                Title = "Visual Studio Saturday 2019",
                Date = new DateTime(2019, 11, 16).ToString("s"),
            };
            response.Events.Add(vsSaturdayEvent);

            // Tech-Pub - Telegram Bot
            //
            var telegramBotEvent = new XeEvent()
            {
                Title = "Tech-Pub - Telegram Bot",
                Date = new DateTime(2019, 11, 29).ToString("s"),
            };
            telegramBotEvent.Sessions.Add(new XeEventSession
            {
                Title = "Sviluppare Bot per Telegram",
                Author = "Diano Bellio"
            });
            response.Events.Add(telegramBotEvent);


            return Task.FromResult(response);
        }
    }
}
