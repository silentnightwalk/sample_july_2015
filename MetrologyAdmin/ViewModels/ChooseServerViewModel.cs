using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MetrologyAdmin
{
    public class ChooseServerViewModel: INotifyPropertyChanged, IBusyContent
    {
        private IEnumerable<RoadServerViewModel> _Servers;
        public IEnumerable<RoadServerViewModel> Servers
        {
            get { return _Servers; }
            set
            {
                _Servers = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Servers"));
            }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsBusy"));
            }
        }

        public string Message { get; private set; }

        private RoadServerViewModel _SelectedServer;
        public RoadServerViewModel SelectedServer
        {
            get { return _SelectedServer; }
            set
            {
                _SelectedServer = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedServer"));
                if (_SelectedServer != null)
                {
                    _settingsManager.SelectedServerId = _SelectedServer.Id;
                    _eventBus.Raise<ServerSelectedEvent>(new ServerSelectedEvent(_SelectedServer));
                }
            }
        }

        //--------------------------------------------------------------------

        private readonly MainService _mainService;
        private readonly IEventBus _eventBus;
        private readonly SettingsManager _settingsManager;

        public ChooseServerViewModel(MainService mainService, IEventBus eventBus, SettingsManager settingsManager)
        {
            _mainService = mainService;
            _eventBus = eventBus;
            _settingsManager = settingsManager;

            LoadData();
        }

        private async void LoadData()
        {
            IsBusy = true;

            try
            {
                Servers = await TaskEx.Run(() => _mainService.GetServersList());
            }
            catch (AggregateException exeption)
            {
                MessageBox.Show(String.Join("\r\n", exeption.InnerExceptions.Select(x => x.Message)));
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            finally
            {
                var select = _settingsManager.SelectedServerId;
                if (select != 0)
                {
                    SelectedServer = Servers.FirstOrDefault(x => x.Id == select);
                }

                if (SelectedServer == null)
                {
                    SelectedServer = Servers.FirstOrDefault();
                }

                IsBusy = false;
            }
        }

        //-------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        
    }
}
