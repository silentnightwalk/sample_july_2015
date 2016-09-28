using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MetrologyAdmin.Requests
{
    public class RequestBus : IRequestBus
    {
        private readonly IContainer Container;

        public RequestBus(IContainer container)
        {
            Container = container;
        }

        public void RegisterHandlerType<THandler, TRequest>()
            where THandler : IRequestHandler<TRequest>
            where TRequest : IRequest
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<THandler>().As<IRequestHandler<TRequest>>();
            builder.Update(Container);
        }

        public void Publish<T>(T request) where T : IRequest
        {
            var handlers = Container.Resolve<IEnumerable<IRequestHandler<T>>>();
            var uiDisaptcher = Application.Current.Dispatcher;

            foreach (var handler in handlers)
            {
                if (uiDisaptcher.CheckAccess())
                {
                    handler.Handle(request);
                }
                else
                {
                    uiDisaptcher.BeginInvoke(new Action(() => handler.Handle(request)));
                }
            }

        }
    }
}
