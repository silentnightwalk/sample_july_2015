using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Server.Core
{
    public interface IConnectionFactory
    {
        IDbConnection Create(int serverId);
    }
}
