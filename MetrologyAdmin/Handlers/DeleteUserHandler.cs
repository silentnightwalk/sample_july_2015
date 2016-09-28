using Cniitei.MVVM;
using MetrologyAdmin.Core;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace MetrologyAdmin
{
    public class DeleteUserHandler: IRequestHandler<DeleteUserRequest>
    {
        private readonly IUserService _userService;

        public DeleteUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public void Handle(DeleteUserRequest request)
        {
            //TODO: put in task + busy indicator

            var mbRes = MessageBox.Show(
                "Вы действительно хотите удалить пользователя " + request.UserName + "?"
                , ""
                , MessageBoxButton.YesNo
                , MessageBoxImage.Question
                );

            if (mbRes == MessageBoxResult.Yes)
            {
                CommitChanges(request);
            }
        }

        public async void CommitChanges(DeleteUserRequest request)
        {
            request.BusyIndicator.IsBusy = true;

            try
            {
                await TaskEx.Run(() => _userService.DeleteExistingUser(request.ServerId,request.UserId));

            }
            catch(Exception E)
            {
                MessageBox.Show("Ошибка: " + E.Message);
            }
            finally
            {
                request.DeleteCallback(new DeleteUserResponse(true));
                request.BusyIndicator.IsBusy = false;
            }

        }
    }
}
