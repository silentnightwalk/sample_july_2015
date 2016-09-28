/// переменные удобнее определять в конфигурации
/// DEBUGDB - включается, если выбрать Debug
/// FAKE - включается, если выбрать Test (см. Project Properties -> Build -> Conditional compilation symbol)
/// Если поменять конфигурацию, то видно будет как включаются/выключаются нужные куски кода

//#define FAKE
//#define REALWORLD
//#define DEBUGDB

using Autofac;
using MetrologyAdmin.Core;
using MetrologyAdmin.Domain;
using MetrologyAdmin.FakeData;
using MetrologyAdmin.ReadModel;
using MetrologyAdmin.Server;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.ApplicationLayer
{
    public class AppInitializer 
    {

        private ContainerBuilder _builder;

        public void Initialize(ContainerBuilder containerBuilder)
        {
            _builder = containerBuilder;

            ConfigureContainer_RealWorld();
        }


        private void ConfigureContainer_RealWorld()
        {

            _builder.RegisterType<MainService>().AsSelf();

#if !FAKE

    #if DEBUGDB
            _builder.RegisterType<DebugServersService>().As<IServersService>().SingleInstance();
    #else
            _builder.RegisterType<ServersService>().As<IServersService>().SingleInstance();
    #endif
            _builder.RegisterType<AuthorizationService>().As<IAuthorizationService>().SingleInstance();
            _builder.RegisterType<ConnectionFactory>().As<IConnectionFactory>();
            _builder.RegisterType<ReadModelFacade>().As<IReadModelFacade>();
            _builder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>();
            _builder.RegisterType<UserService>().As<IUserService>();
#else
            _builder.RegisterType<FakeServersService>().As<IServersService>().SingleInstance();
            _builder.RegisterType<FakeAuthorizationService>().As<IAuthorizationService>().SingleInstance();
            _builder.RegisterType<FakeReadModelFacade>().As<IReadModelFacade>();
            //_builder.RegisterType<MainService>().AsSelf();
            _builder.RegisterType<FakeUserService>().As<IUserService>();
#endif
            _builder.RegisterType<EmailService>().AsSelf();
        }



    }
}
