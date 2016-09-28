using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Events
{
    public interface IEventBus
    {
        void RegisterHandler<TEvent>(IEventHandler<TEvent> handler)
            where TEvent:IEvent;

        void Raise<TEvent>(TEvent @event)
            where TEvent : IEvent;

    }
}
