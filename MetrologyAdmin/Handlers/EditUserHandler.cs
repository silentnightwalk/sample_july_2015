using Cniitei.MVVM;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class EditUserHandler: IRequestHandler<EditUserRequest>
    {
        private readonly IDialogService _dialogService;
        private readonly AddEditUserViewFactory _viewFactory;

        public EditUserHandler(IDialogService dialogServcie, AddEditUserViewFactory viewFactory)
        {
            _dialogService = dialogServcie;
            _viewFactory = viewFactory;
        }

        public void Handle(EditUserRequest request)
        {
            var view = _viewFactory.CreateEditUserView(request);
            _dialogService.ShowDialog<EditUserResponse>(view, request.Callback);
        }

    }
}
