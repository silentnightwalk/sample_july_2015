using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Requests
{
    public interface IRequestHandler<T>
        where T : IRequest
    {
        void Handle(T request);
    }
}
