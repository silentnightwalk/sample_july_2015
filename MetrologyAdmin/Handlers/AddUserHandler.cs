using Cniitei.MVVM;
using MetrologyAdmin.Core;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class AddUserHandler : IRequestHandler<AddUserRequest>
    {
        private readonly IDialogService _dialogService;
        private readonly AddEditUserViewFactory _viewFactory;

        public AddUserHandler(IDialogService dialogServcie, AddEditUserViewFactory viewFactory)
        {
            _dialogService = dialogServcie;
            _viewFactory = viewFactory;
        }

        public void Handle(AddUserRequest request)
        {
            var view = _viewFactory.CreateAddUserView(request);
            _dialogService.ShowDialog<AddUserResponse>(view, request.Callback);
        }
    }
}
