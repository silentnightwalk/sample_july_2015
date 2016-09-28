using Cniitei.MVVM;
using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MetrologyAdmin
{
    public class AddEditUserViewModel<TResponse>: IDialogCloseable<TResponse>, INotifyPropertyChanged
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

        private string _BusyMessage;
        public string BusyMessage
        {
            get { return _BusyMessage; }
            set 
            { 
                _BusyMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("BusyMessage"));
            }
        }

        private string _Error;
        public string Error 
        { 
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Error"));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }

        public bool HasError
        {
            get
            {
                return !String.IsNullOrEmpty(Error);
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set 
            { 
                _Title = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Fio;
        public string Fio
        {
            get { return _Fio; }
            set
            {
                _Fio = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Fio"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set 
            { 
                _Login = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Login"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private string _AccessCode;
        public string AccessCode
        {
            get { return _AccessCode; }
            set 
            { 
                _AccessCode = value; 
                PropertyChanged(this, new PropertyChangedEventArgs("AccessCode"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private Role _SelectedRole;
        public Role SelectedRole
        {
            get { return _SelectedRole; }
            set
            {
                _SelectedRole = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedRole"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private Role[] _AllRoles;
        public Role[] AllRoles
        {
            get { return _AllRoles; }
            set
            {
                _AllRoles = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AllRoles"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private string _SelectedOrganizationName;
        public string SelectedOrganizationName
        {
            get { return _SelectedOrganizationName; }
            set
            {
                _SelectedOrganizationName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOrganizationName"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        protected int SelectedOrganizationId { get; set; }

        private string _Post;
        public string Post
        {
            get { return _Post; }
            set
            {
                _Post = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Post"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                PropertyChanged(this, new PropertyChangedEventArgs("SendLoginMessageCheckEnabled"));
                PropertyChanged(this, new PropertyChangedEventArgs("SendLoginMessage"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private string _Telephone;
        public string Telephone
        {
            get { return _Telephone; }
            set
            {
                _Telephone = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Telephone"));
                if (OkCommand != null) OkCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _SendLoginMessage = false;
        public bool SendLoginMessage
        {
            get 
            {
                //if (String.IsNullOrWhiteSpace(this.Email))
                //    return false;
                //else
                    //return _settingsManager.SendMessageWhenLoginDetailsChanged; 
                    return _SendLoginMessage;
            }
            set
            {
                //_settingsManager.SendMessageWhenLoginDetailsChanged = value;
                _SendLoginMessage = value;
            }
        }

        public bool SendLoginMessageCheckEnabled
        {
            get { return !String.IsNullOrWhiteSpace(this.Email); }
        }

        

        //----------------------------------------

        public Command BrowseOrganizationCommand { get; private set; }
        public Command GeneratePasswordCommand { get; private set; }

        private bool CanBrowseOrganization(object input)
        {
            return true;
        }

        private void ExecuteBrowseOrganization(object input)
        {
            var request = new SelectOrganizationRequest(
                _serverId,
                (response) =>
                {
                    SelectedOrganizationName = response.SelectedOrganization.Name;
                    SelectedOrganizationId = response.SelectedOrganization.Id;
                },
                SelectedOrganizationId
            );

            _requestBus.Publish(request);
        }

        private bool CanGenerate(object input)
        {
            return true;
        }

        private void ExecuteGeneratePassword(object input)
        {
            this.AccessCode = PasswordGenerator.GeneratePassword();
        }

        //----------------------------------------

        protected int _serverId;

        protected readonly IEventBus _eventBus;
        protected readonly IRequestBus _requestBus;
        protected readonly IUserService _userService;
        protected readonly SettingsManager _settingsManager;
        protected readonly EmailService _emailService;

        public AddEditUserViewModel(
            int serverId
            , int? selectedOrganizationId
            , string selectedOrganizationName
            , IEventBus eventBus
            , IRequestBus requestBus
            , IReadModelFacade readModel
            , SettingsManager settingsManager
            , IUserService userService
            , EmailService emailService
            )
        {
            _eventBus = eventBus;
            _requestBus = requestBus;
            _serverId = serverId;
            _userService = userService;
            _settingsManager = settingsManager;
            _emailService = emailService;

            if (selectedOrganizationId != null)
                this.SelectedOrganizationId = selectedOrganizationId.Value;

            if (!String.IsNullOrWhiteSpace(selectedOrganizationName))
                this.SelectedOrganizationName = selectedOrganizationName;

            CancelCommand = new Command((x) => true,(x) => DialogClose(this,null));
            BrowseOrganizationCommand = new Command(CanBrowseOrganization, ExecuteBrowseOrganization);
            GeneratePasswordCommand = new Command(CanGenerate,ExecuteGeneratePassword);
            AllRoles = readModel.GetAllRoles(serverId);
        }

        public event DialogCloseEvent<TResponse> DialogClose = delegate { };

        public ICommand CancelCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaiseDialogClose(object sender, bool? dialogResult, TResponse result)
        {
            DialogClose(sender, new DialogCloseEventArgs<TResponse>(dialogResult, result));
        }

        public Command OkCommand { get; protected set; }

        protected bool CanExecuteOk(object input)
        {
            bool canExecute = true;

            canExecute = canExecute && !String.IsNullOrWhiteSpace(Login);
            canExecute = canExecute && !String.IsNullOrWhiteSpace(AccessCode);
            canExecute = canExecute && SelectedRole != null;
            canExecute = canExecute && SelectedOrganizationName != null;
            canExecute = canExecute && SelectedOrganizationId != 0;
            canExecute = canExecute && _serverId != 0;

            return canExecute;
        }
    }


    //-------------------------------------------------------------------------------------------------------------


    public class AddUserViewModel : AddEditUserViewModel<AddUserResponse>
    {
        public AddUserViewModel(AddUserRequest request, IEventBus eventBus, IRequestBus requestBus, IReadModelFacade readModel, SettingsManager settingsManager, IUserService userService, EmailService emailService)
            : base(request.ServerId, request.SelectedOrganization.Id,request.SelectedOrganization.Name,  eventBus, requestBus, readModel,settingsManager, userService, emailService)
        {
            OkCommand = new Command(CanExecuteOk,ExecuteOk );
            Title = "Добавление пользователя";
        }

        private void ExecuteOk(object input)
        {
            var addUserResponse = new AddUserResponse
                {
                    NewUser = new UserDto(
                        new User 
                        { 
                            EMail = this.Email,
                            Login = this.Login,
                            Name = this.Fio,
                            Organization = this.SelectedOrganizationName,
                            OrganizationId = this.SelectedOrganizationId,
                            Post = this.Post,
                            RoleId = this.SelectedRole.Id,
                            Telephone = this.Telephone
                        }
                        ,_serverId
                        ,this.AccessCode
                        )
                };

            CommitChanges(addUserResponse);
            
        }

        private async void CommitChanges(AddUserResponse response)
        {
            IsBusy = true;
            Error = null;

            try
            {
                await TaskEx.Run(() => _userService.CreateNewUser(response.NewUser));
                if (!String.IsNullOrWhiteSpace(response.NewUser.EMail)
                    && SendLoginMessage)
                    await TaskEx.Run(() => _emailService.SendUserDetails(response.NewUser));
                RaiseDialogClose(this, true, response);
            }
            catch (Exception E)
            {
                Error = E.Message;
                //MessageBox.Show("Ошибка: " + E.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }

    //------------------------------------------------------------------------------------------

    public class EditUserViewModel : AddEditUserViewModel<EditUserResponse>
    {
        private int _id;
        private readonly string _defaultLogin;
        private readonly string _defaultAccessCode;
        private readonly string _defaultName;
        private readonly int?   _defaultRoleId;
        private readonly int?   _defaultOrganizationId;
        private readonly string _defaultPost;
        private readonly string _defaultEmail;
        private readonly string _defaultTelephone;

        public EditUserViewModel(EditUserRequest request, IEventBus eventBus, IRequestBus requestBus, IReadModelFacade readModel, SettingsManager settingsManager, IUserService userService, EmailService emailService)
            : base(request.DefaultUserDetails.ServerId, request.DefaultUserDetails.OrganizationId,request.DefaultUserDetails.Organization, eventBus, requestBus, readModel, settingsManager, userService, emailService)
        {
            this._id = request.DefaultUserDetails.Id;
            this.Fio = request.DefaultUserDetails.Name;
            this.Login = request.DefaultUserDetails.Login;
            this.AccessCode = request.DefaultUserDetails.AccessCode;
            this.SelectedRole = AllRoles.FirstOrDefault(x => x.Id == request.DefaultUserDetails.RoleId);
            this.SelectedOrganizationId = request.DefaultUserDetails.OrganizationId.HasValue ? request.DefaultUserDetails.OrganizationId.Value : 0;
            this.SelectedOrganizationName = request.DefaultUserDetails.Organization;
            this.Post = request.DefaultUserDetails.Post;
            this.Email = request.DefaultUserDetails.EMail;
            this.Telephone = request.DefaultUserDetails.Telephone;

            _defaultLogin = Login;
            _defaultAccessCode = AccessCode;

            _defaultLogin = this.Login;
            _defaultAccessCode = this.AccessCode;
            _defaultName = this.Fio;
            _defaultRoleId = this.SelectedRole == null ? default(int?) : this.SelectedRole.Id;
            _defaultOrganizationId = this.SelectedOrganizationId;
            _defaultPost = this.Post;
            _defaultEmail = this.Email;
            _defaultTelephone = this.Telephone;

            OkCommand = new Command(CanExecuteOk, ExecuteOk);

            Title = "Редактирование пользователя";
        }


        private bool SomethingChanged(UserDto dto)
        {
            return false
                || _defaultLogin != dto.Login
                || _defaultAccessCode != dto.AccessCode
                || _defaultName != dto.Name
                || _defaultRoleId != dto.RoleId
                || _defaultOrganizationId != dto.OrganizationId
                || _defaultPost != dto.Post
                || _defaultEmail != dto.EMail
                || _defaultTelephone != dto.Telephone
                ;
        }

        private void ExecuteOk(object input)
        {
            var editedUser = new UserDto(
                        new User
                        {
                            Id = this._id,
                            EMail = this.Email,
                            Login = this.Login,
                            Name = this.Fio,
                            Organization = this.SelectedOrganizationName,
                            OrganizationId = this.SelectedOrganizationId,
                            Post = this.Post,
                            RoleId = this.SelectedRole.Id,
                            Telephone = this.Telephone
                        }
                        , _serverId
                        , this.AccessCode
                        );

            if (!SomethingChanged(editedUser)) RaiseDialogClose(this,false,null);
            else
            {
                var editUserResponse = new EditUserResponse{ EditedUser = editedUser };

                CommitChanges(editUserResponse);
            }
            
        }

        private async void CommitChanges(EditUserResponse response)
        {
            BusyMessage = "Редактирование...";
            IsBusy = true;
            Error = null;

            try
            {
                //TODO: put in task
                await TaskEx.Run(() => _userService.EditExistingUser(response.EditedUser));
                if (
                    !String.IsNullOrWhiteSpace(response.EditedUser.EMail)
                    && (_defaultLogin != response.EditedUser.Login
                    || _defaultAccessCode != response.EditedUser.AccessCode
                    )
                    && SendLoginMessage
                    )
                    await TaskEx.Run(() => _emailService.SendUserDetails(response.EditedUser.ServerId, response.EditedUser.Id));
                RaiseDialogClose(this, true, response);
            }
            catch (Exception E)
            {
                Error = E.Message;
                //MessageBox.Show("Ошибка: " + E.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        
    }
}
