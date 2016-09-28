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
    /// Interaction logic for MetrologistsControl.xaml
    /// </summary>
    public partial class MetrologistsView : UserControl
    {
        public MetrologistsView(MetrologistsViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        private void lvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as UserViewModel;
            var vm = DataContext as MetrologistsViewModel;
            if (item != null && vm != null)
            {
                vm.ExecuteEdit(null);
            }
        }
    }
}
