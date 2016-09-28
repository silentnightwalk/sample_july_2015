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
    /// Interaction logic for OrganizationTreeControl.xaml
    /// </summary>
    public partial class OrganizationTreeView : UserControl
    {
        public OrganizationsTreeViewModel VM
        {
            get { return this.DataContext as OrganizationsTreeViewModel; }
            set { this.DataContext = value; }
        }


        public OrganizationTreeView(OrganizationsTreeViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue != null)
                VM.SelectedOrganization = e.NewValue as Organization;
        }
    }
}
