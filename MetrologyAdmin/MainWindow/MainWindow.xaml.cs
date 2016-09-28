using Autofac;
using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MetrologyAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(
                ChooseServerView serversControl, 
                OrganizationTreeViewFactory organizationsControlFactory,
                MetrologistsView metrologistsControl
            )
        {
            InitializeComponent();

            var organizationTreeControl = organizationsControlFactory.CreateOrganizationTreeView();

            this.ServersHolder.Content = serversControl;
            this.OrganizationsTreeHolder.Content = organizationTreeControl;
            this.UsersHolder.Content = metrologistsControl;

            this.DataContext = 
                new BusyUmbrella(
                    serversControl,
                    organizationTreeControl,
                    metrologistsControl
                    );
        }
    }
}
