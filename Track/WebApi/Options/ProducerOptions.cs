﻿namespace WebApi.Options
{
    public class ProducerOptions
    {
        public const string Name = "ProducerOptions";
        public string AppId { get; set; }
        public ProducerReactionOptions Reactions { get; set; }
    }
}
