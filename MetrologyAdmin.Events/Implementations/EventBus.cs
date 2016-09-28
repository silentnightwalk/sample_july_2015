using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetStuffs.SimpleEvents;

namespace MetrologyAdmin.Events
{
    public class EventBus : IEventBus
    {
        private readonly Aggregator Aggregator = new Aggregator();

        public void RegisterHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            Aggregator.Subscribe<TEvent>(handler, handler.Handle, this);
        }

        public void Raise<TEvent>(TEvent @event) where TEvent : IEvent
        {
            Aggregator.Publish(@event, this);
        }
    }
}
