using Cniitei.MVVM.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using MetrologyAdmin.Core;
using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Events;

namespace MetrologyAdmin
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        private MainService _mainService;
        private readonly Action _onLogin;
        private readonly IEventBus _eventBus;
        private readonly SettingsManager _settingsManager;

        public LoginViewModel(Action onLogin, MainService mainService, IEventBus eventBus, SettingsManager settingsManager)
        {
            _mainService = mainService;

            _onLogin = onLogin;

            _eventBus = eventBus;

            _settingsManager = settingsManager;

            LoadServersList();

            LoginCommand = new DependentCommand(
                    LoginUser,
                    CanUserLogin
                )
                .WatchFor(this, x => x.IsBusy)
                .WatchFor(this, x => x.Login)
                .WatchFor(this, x => x.Password)
                .WatchFor(this, x => x.SelectedServer);
        }

        //---------------------------------------------------------

        private RoadServerViewModel[] _ServersList;
        public RoadServerViewModel[] ServersList
        {
            get { return _ServersList; }
            set
            {
                _ServersList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ServersList"));
            }
        }

        private RoadServerViewModel _SelectedServer;
        public RoadServerViewModel SelectedServer
        {
            get { return _SelectedServer; }
            set
            {
                _SelectedServer = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedServer"));
            }
        }

        private SecureString _Password;
        public SecureString Password
        {
            get { return _Password; }
            set { _Password = value; PropertyChanged(this, new PropertyChangedEventArgs("Password")); }
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Login"));
            }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            private set
            {
                _Message = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Message"));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }

        public bool HasError
        {
            get
            {
                return !String.IsNullOrEmpty(Message);
            }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            private set
            {
                _IsBusy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public string Title
        {
            get { return "Подсистема администрирования пользователей"; }
        }

        public string AppName
        {
            get { return "Подсистема администрирования пользователей"; }
        }

        public string Site
        {
            get { return ""; }
        }

        public Uri Email
        {
            get { return new Uri(""); }
        }

        public Uri IntranetEmail
        {
            get { return new Uri(""); }
        }

        public string CompanyName
        {
            get { return "ООО \"ЦНИИТЭИ-ИС\""; }
        }

        public ICommand LoginCommand { get; private set; }


        //--------------------------------------------------------------

        private async void LoadServersList()
        {
            IsBusy = true;

            try
            {
                ServersList = await TaskEx.Run(()=> _mainService.GetServersList());
            }
            catch (AggregateException exeption)
            {
                Message = String.Join("\r\n", exeption.InnerExceptions.Select(x => x.Message));
            }
            catch (Exception exeption)
            {
                Message = exeption.Message;
            }
            finally
            {
                var select = _settingsManager.SelectedLoginServerId;
                if (select != 0)
                {
                    SelectedServer = ServersList.FirstOrDefault(x => x.Id == (int)select);
                }
                IsBusy = false;
            }
        }


        private async void LoginUser()
        {
            IsBusy = true;
            var unsecurePwd = ConvertToUnsecureString(Password);
   

            try
            {
                var result = await TaskEx.Run(() => _mainService.Authorize(_SelectedServer.Id, _Login, unsecurePwd));
                if (result)
                {
                    _settingsManager.SelectedLoginServerId = _SelectedServer.Id;
                    //_eventBus.Raise<AuthenticatedEvent>(new AuthenticatedEvent());
                    if (_onLogin != null) _onLogin();
                }
                else
                {
                    Message = "Пользователь не найден.";
                }
            }
            catch (AggregateException exeption)
            {
                Message = String.Join("\r\n", exeption.InnerExceptions.Select(x => x.Message));
            }
            catch (Exception exeption)
            {
                Message = exeption.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanUserLogin()
        {
            return !(
                   IsBusy
                || String.IsNullOrWhiteSpace(Login)
                || Password == null
                || Password.Length == 0
                || SelectedServer == null
                );
        }

        public static string ConvertToUnsecureString(SecureString secureString)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }


        //----------------------------------------------------------


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
