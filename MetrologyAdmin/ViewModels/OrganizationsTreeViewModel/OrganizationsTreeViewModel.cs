using Cniitei.MVVM;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MetrologyAdmin
{
    public partial class OrganizationsTreeViewModel : INotifyPropertyChanged, 
        IBusyContent, 
        IEventHandler<ServerSelectedEvent>, 
        IDialogCloseable<SelectOrganizationResponse>
    {
        private readonly IReadModelFacade _readModelFacade;
        private readonly IEventBus _eventBus;
        private readonly SettingsManager _settingsManager;
        private readonly int? _defaultOrganizationId;

        private int? SelectedServerId { get; set; }

        private Organization _SelectedOrganization;
        public Organization SelectedOrganization
        {
            get { return _SelectedOrganization; }
            set
            {
                _SelectedOrganization = value;
                _eventBus.Raise<OrganizationSelectedEvent>(new OrganizationSelectedEvent(_SelectedOrganization, SelectedServerId.Value));

                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrganization"));
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

        public string Message { get; set; }

        public OrganizationsTreeViewModel(IReadModelFacade readModelFacade, IEventBus eventBus, SettingsManager settingsManager, int? defaultOrganizationId = null, int? serverId = null)
        {
            _readModelFacade = readModelFacade;
            _eventBus = eventBus;
            _settingsManager = settingsManager;
            _defaultOrganizationId = defaultOrganizationId;

            _eventBus.RegisterHandler<ServerSelectedEvent>(this);

            CancelCommand = new Command(CanCancel, Cancel);

            OkCommand = new Cniitei.MVVM.Command.DependentCommand(Ok, CanOk)
                .WatchFor(this, x => x.SelectedOrganization);

            ChangeServer(serverId);
        }

        public void Handle(ServerSelectedEvent args)
        {
            ChangeServer(args.SelectedServer.Id);
        }

        private void ChangeServer(int? serverId)
        {
            SelectedServerId = serverId;
            LoadData(SelectedServerId);
        }

        private async void LoadData(int? serverId, int? defaultSelectionId = null)
        {
            if (!serverId.HasValue)
            {
                OrganizationTree = null;
                return;
            }

            IsBusy = true;
            
            try
            {
                OrganizationTree = null;
                OrganizationTree = await TaskEx.Run(() => _readModelFacade.GetOrganizationsTree(serverId.Value).Select(o=>new OrganizationViewModel(o)).ToArray());
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
                if (OrganizationTree != null)
                {
                    if (_defaultOrganizationId.HasValue && _defaultOrganizationId != null)
                        ExpandAndSelect(_defaultOrganizationId.Value);
                }
                IsBusy = false;
            }
        }

        private void ExpandAndSelect()
        {
            var item = (OrganizationViewModel)SelectedOrganization;

            if (item != null)
            {
                item.IsSelected = true;

                while (item.Parent != null)
                {
                    item = item.Parent as OrganizationViewModel;
                    item.IsExpanded = true;
                    Debug.WriteLine(String.Format("Expanded item. Name: {0}, Hash: {1}", item.Name, item.GetHashCode()));
                }
            }
        }

        private void ExpandAndSelect(int organizationToSelectId)
        {
             
            var orgToSelect = OrganizationViewModel
                .AsEnumerable(this.OrganizationTree)
                .FirstOrDefault(x => x.Id == organizationToSelectId)
                as OrganizationViewModel
                ;

            if (orgToSelect != null)
            {
                SelectedOrganization = orgToSelect;
                ExpandAndSelect();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        public event DialogCloseEvent<SelectOrganizationResponse> DialogClose;
       
        private bool CanOk()
        {
            return SelectedOrganization != null;
        }

        private void Ok()
        {
            if (SelectedOrganization != null)
            {
                var response = new SelectOrganizationResponse(SelectedOrganization);
                DialogClose(this, new DialogCloseEventArgs<SelectOrganizationResponse>(true, response));
            }
        }

        private bool CanCancel(object input)
        {
            return true;
        }

        private void Cancel(object input)
        {
            DialogClose(this, new DialogCloseEventArgs<SelectOrganizationResponse>(false, null));
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
    }
}
