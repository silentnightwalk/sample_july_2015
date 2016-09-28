using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MetrologyAdmin
{
    public partial class MetrologistsViewModel  
        :IBusyContent
        ,IEventHandler<OrganizationSelectedEvent>
        ,IEventHandler<ServerSelectedEvent>
    {
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

        private bool _AggregateOn = true;
        public bool AggregateOn
        {
            get { return _AggregateOn; }
            set
            {
                bool aggregateToggleChanged = _AggregateOn != value;

                _AggregateOn = value;

                if (aggregateToggleChanged && _serverId != 0 && _organization != null)
                    LoadData(_serverId, _organization.Id);

                PropertyChanged(this, new PropertyChangedEventArgs("AggregateOn"));
            }
        }

        private readonly IReadModelFacade _readModel;
        private readonly IUserService _userService;
        private readonly IEventBus _eventBus;
        private readonly IRequestBus _requestBus;
        private readonly EmailService _emailService;
        
        public MetrologistsViewModel(
            IReadModelFacade readModel
            , IUserService userService
            , IEventBus eventBus
            , IRequestBus requestBus
            , EmailService emailService
            )
        {
            //TODO: set title
            _readModel = readModel;
            _userService = userService;
            _eventBus = eventBus;
            _requestBus = requestBus;
            _emailService = emailService;

            _eventBus.RegisterHandler<ServerSelectedEvent>(this);
            _eventBus.RegisterHandler<OrganizationSelectedEvent>(this);
            
            InitializeCommands();
        }

      
        private int _serverId;
        private Organization _organization;

        public Organization SelectedOrganization
        {
            get
            {
                return _organization;
            }
            private set
            {
                _organization = value;

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrganization"));
            }
        }

        private UserViewModel _SelectedUser;
        public UserViewModel SelectedUser
        {
            get { return _SelectedUser; }
            set
            {
                _SelectedUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedUser"));
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
                EmailCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<UserViewModel> _Users;
        public ObservableCollection<UserViewModel> Users
        {
            get { return _Users; }
            set
            {
                _Users = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Users"));
                AddCommand.RaiseCanExecuteChanged();
                ReportCommand.RaiseCanExecuteChanged();
                ApplyFilterCommand.RaiseCanExecuteChanged();
            }
        }

        public string Message { get; private set; }

        public object LastClickedColumn { get; set; }

        public void Handle(OrganizationSelectedEvent args)
        {
            _serverId = args.SelecterServerId;
            SelectedOrganization = args.SelectedOrganization;

            if (_organization != null)
                LoadData(_serverId, _organization.Id);
        }

        //TODO: server refactoring
        private RoadServerViewModel _server { get; set; }

        public void Handle(ServerSelectedEvent args)
        {
            //_serverId = args.SelectedServer.Id;
            _server = args.SelectedServer;
            Users = null;
            SelectedOrganization = null;
        }

        private async void LoadData(int serverId, int organizationId)
        {
            IsBusy = true;

            try
            {
                Users = null;

                var data = await TaskEx.Run(
                    () => _readModel
                                .GetUsersByOrganization(serverId, organizationId, AggregateOn)
                                .Select(user => new UserViewModel(user)) 
                                );
                Users = new ObservableCollection<UserViewModel>(data);
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
                ApplyFilterCommand_Execute();
                IsBusy = false;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged = delegate { };   
    }
     

}
