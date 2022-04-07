using Extensions;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Options;
using Rabbit;
using RabbitMQ.Abstraction;
using RabbitMQ.Client;
using System;

namespace Modules
{
    public class RabbitModule : Module
    {
        public override void Load(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqProducer<EventMessageBody>, ProducerEvent>();
            services.AddSingleton<IRabbitMqProducer<ReactionMessageBody>, ProducerReaction>();

            services.AddSingleton(serviceProvider =>
            {
                return new ConnectionFactory
                {
                    UserName = "sa",
                    Password = "Password1",
                    HostName = "localhost",
                    Port = 5800,
                    VirtualHost = "/",
                    ContinuationTimeout = new TimeSpan(10, 0, 0, 0),
                };
            });

            services.Configure<ProducerOptions>(Configuration.GetSection(ProducerOptions.Name));
        }
    }
}
