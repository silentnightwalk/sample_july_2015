using Autofac;
using Cniitei.MVVM;
using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace MetrologyAdmin
{
    public class Bootstrapper
    {
        private IContainer Container { get; set; }

        public void Run()
        {
            PreConfigureContainer();

            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (Login())
            {
                ConfigureContainer();
                RegisterRequestHandlers();

                Application.Current.MainWindow = Container.Resolve<MainWindow>();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Application.Current.MainWindow.Show();
            }
        }

        private void PreConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var appInitializer = new AppInitializer();
            appInitializer.Initialize(builder);

            builder.RegisterType<SettingsManager>().AsSelf().SingleInstance();
            builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();
            builder.RegisterType<LoginWindow>().AsSelf();

            Container = builder.Build();

        }

        private bool Login()
        {
            LoginWindow loginWindow = Container.Resolve<LoginWindow>();

            var canStart = (loginWindow.ShowDialog() == true);

            if (!canStart)
            {
                Application.Current.Shutdown();
            }

            return canStart;
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RequestBus>().As<IRequestBus>().SingleInstance();

            builder.RegisterType<DialogService>().As<IDialogService>();

            builder.RegisterType<AddEditUserViewFactory>().AsSelf();
            builder.RegisterType<OrganizationTreeViewFactory>().AsSelf();

            builder.RegisterType<ChooseServerViewModel>().AsSelf();

            builder.RegisterType<MetrologistsViewModel>().AsSelf();

            builder.RegisterType<ChooseServerView>().AsSelf();
            builder.RegisterType<MetrologistsView>().AsSelf();
            
            builder.RegisterType<MainWindow>().AsSelf();

            builder.Register<IContainer>((ctx) => this.Container);

            builder.Update(Container);
            
        }

        protected void RegisterRequestHandlers()
        {
            var requestBus = Container.Resolve<IRequestBus>();

            requestBus.RegisterHandlerType<AddUserHandler, AddUserRequest>();
            requestBus.RegisterHandlerType<EditUserHandler, EditUserRequest>();
            requestBus.RegisterHandlerType<DeleteUserHandler, DeleteUserRequest>();
            requestBus.RegisterHandlerType<SelectOrganizationHandler, SelectOrganizationRequest>();

        }


    }
}
