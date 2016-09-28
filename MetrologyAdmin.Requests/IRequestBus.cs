using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Requests
{
    public interface IRequestBus
    {
        void RegisterHandlerType<THandler, TRequest>()
            where THandler : IRequestHandler<TRequest>
            where TRequest : IRequest;

        void Publish<T>(T request)
            where T : IRequest;

    }
}
