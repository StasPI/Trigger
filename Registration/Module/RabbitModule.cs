using Extensions;
using Messages;
using Microsoft.Extensions.Configuration;
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

            RabbitMQOptions settings = Configuration.GetSection(RabbitMQOptions.Name).Get<RabbitMQOptions>();

            services.AddSingleton<IRabbitMqProducer<EventMessageBody>, ProducerEvent>();
            services.AddSingleton<IRabbitMqProducer<ReactionMessageBody>, ProducerReaction>();

            services.AddSingleton(serviceProvider =>
            {
                return new ConnectionFactory
                {
                    UserName = settings.UserName,
                    Password = settings.Password,
                    HostName = settings.HostName,
                    Port = settings.Port,
                    VirtualHost = settings.VirtualHost,
                    ContinuationTimeout = new TimeSpan(10, 0, 0, 0),
                };
            });

            services.Configure<RabbitMQOptions>(Configuration.GetSection(RabbitMQOptions.Name));
        }
    }
}
