using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Events;
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
using System.Windows.Shapes;

namespace MetrologyAdmin
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginViewModel Model
        {
            get { return DataContext as LoginViewModel; }
            set { DataContext = value; }
        }

        public LoginWindow(MainService mainService, IEventBus eventBus, SettingsManager settingsManager)
        {
            Model = new LoginViewModel(
                () =>
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        this.DialogResult = true;
                    }
                    else
                    {
                        this.Dispatcher.Invoke((Action)(() => this.DialogResult = true));
                    }
                },
                mainService,
                eventBus,
                settingsManager
            );

            InitializeComponent();

            this.Loaded += LoginWindow_Loaded;

            this.PreviewKeyDown += LoginWindow_PreviewKeyDown;
        }

        void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(tbLogin);
        }

        void LoginWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
        }

        private void MoveFocusToNext(KeyEventArgs e)
        {
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
            TraversalRequest request = new TraversalRequest(focusDirection);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }

        private void PasswordControl_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb != null && Model != null)
            {
                Model.Password = pb.SecurePassword;
            }
        }

        private void tbLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoveFocusToNext(e);
            }
        }

        private void TextElement_GotFocus(object sender, RoutedEventArgs e)
        {
            var element = sender as TextBox;
            if (element != null)
            {
                element.SelectAll();
            }
            else
            {
                var pb = sender as PasswordBox;
                if (pb != null)
                {
                    pb.SelectAll();
                }
            }
        }
    }
}
