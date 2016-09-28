using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Events
{
    public interface IEventHandler<T>
        where T : IEvent
    {
        void Handle(T @event);
    }
}
