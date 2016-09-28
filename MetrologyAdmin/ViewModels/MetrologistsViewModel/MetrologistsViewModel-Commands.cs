using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using MetrologyAdmin.Core;
using Cniitei.Reporting;
using System.Windows.Data;
using System.Reflection;
using System.IO;
using System.Diagnostics.Contracts;

namespace MetrologyAdmin
{
    public partial class MetrologistsViewModel 
    {
        public Command AddCommand { get; private set; }
        public Command EditCommand { get; private set; }
        public Command DeleteCommand { get; private set; }
        public Command ReportCommand { get; private set; }
        public Command VisualizeFilterPanelCommand { get; private set; }
        public Command EmailCommand { get; private set; }
        public Command ApplyFilterCommand { get; private set; }
        public Command ClearFilterCommand { get; private set; }


        public void InitializeCommands()
        {
            AddCommand = new Command(CanExecuteAdd,ExecuteAdd);
            EditCommand = new Command(CanExecuteEdit,ExecuteEdit);
            DeleteCommand = new Command(CanExecuteDelete,ExecuteDelete);
            EmailCommand = new Command(CanExecuteEmail, ExecuteEmail);
            ReportCommand = new Command(CanExecuteReport,ExecuteReport);

            InitFilterCommands();
        }
        

        //-------------Add edit delete--------------

        private void ExecuteAdd(object input)
        {
            var addUserRequest = new AddUserRequest(
                (response) =>
                {
                    ProcessAddAction(
                        response.NewUser.ServerId
                        , response.NewUser.Login
                        , response.NewUser.AccessCode
                        );
                }
            )
            {
                SelectedOrganization = _organization,
                ServerId = _serverId
            };

            _requestBus.Publish(addUserRequest);
        }

        private bool CanExecuteAdd(object input)
        {
            return _serverId != 0 && _organization != null;
        }

        public void ExecuteEdit(object input)
        {
            var editUserRequest = new EditUserRequest(
                (response) =>
                {
                    ProcessEditAction(response.EditedUser.ServerId, response.EditedUser.Id);   
                }
            )
            {
                DefaultUserDetails = _userService.GetUserDto(_serverId, SelectedUser != null ? SelectedUser.Id : 0)
            };

            _requestBus.Publish<EditUserRequest>(editUserRequest);
        }

        private bool CanExecuteEdit(object input)
        {
            return SelectedUser != null;
        }

        private void ExecuteDelete(object input)
        {
            var deleteUserRequest = new DeleteUserRequest(
                _serverId
                , SelectedUser.Id
                , SelectedUser.Name
                , this
                , (r) =>
                {
                    if (r.AdminAgreedToDelete)
                        ProcessDeleteAction(_serverId, SelectedUser.Id);
                }
                );
            _requestBus.Publish<DeleteUserRequest>(deleteUserRequest);
            
        }

        private bool CanExecuteDelete(object input)
        {
            return SelectedUser != null;
        }


        //--------------------report email ------------- 

        private bool CanExecuteEmail(object input)
        {
            return _server != null && SelectedUser != null && !String.IsNullOrWhiteSpace(SelectedUser.EMail);
        }

        private async void ExecuteEmail(object input)
        {
            Contract.Assert(_server != null);

            //IsBusy = true;
            try
            {
                await TaskEx.Run(()=>_emailService.SendUserDetails(_server.Id, SelectedUser.Id));
            }
            catch(Exception E)
            {
                MessageBox.Show("Ошибка: " + E.Message);
            }
            finally
            {
                //IsBusy = false;
            }
        }

        private async void ExecuteReport(object input)
        {

            IsBusy = true;

            try
            {
                var reportViewModel = GetReportViewModel();
                await TaskEx.Run(() => ExcelTemplateService.RunAndOpenAsync(".\\ReportTemplate\\ReportTemplate.xlsx", reportViewModel));
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message);
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        /// <summary>
        /// Генерация ReportViewModel (Запускть в UI-потоке, иначе не сработает фильтрация)
        /// </summary>
        /// <returns>ReportViewModel</returns>
        private ReportViewModel GetReportViewModel()
        {
            var reportName = AggregateOn
                ? "Агрегированный отчет"
                : "Отчет";

            reportName += " по пользователям АРМ Метролога";

            //Фильтрация
            var filteredUsers = Users == null
                ? Enumerable.Empty<User>()
                : CollectionViewSource.GetDefaultView(Users).Cast<User>();

            return new ReportViewModel(
                reportName
                ,_server.Name
                , _organization.Name
                , filteredUsers.Select((user, index) => new ExcelUser(user) { Order = index + 1 }).ToArray()
                , IsFilterActive ? "Условия фильтрации:  " + CurrentFilterValue.ToString() : String.Empty
                );
        }


        private bool CanExecuteReport(object input)
        {
            return Users != null && Users.Count > 0 && _organization != null;
        }

        //------------------------------------------

        private bool _FilterVisible;
        public bool FilterVisible
        {
            get { return _FilterVisible; }
            set 
            { 
                _FilterVisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FilterVisible"));
            }
        }
        
       
    }
}
