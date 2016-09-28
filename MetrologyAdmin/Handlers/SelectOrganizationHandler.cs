using Autofac;
using Cniitei.MVVM;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class SelectOrganizationHandler : IRequestHandler<SelectOrganizationRequest>
    {
        private readonly IDialogService _DialogService;
        private readonly IContainer _Container;
        private readonly OrganizationTreeViewFactory _OrganizationTreeViewFactory;

        public SelectOrganizationHandler(IDialogService dialogService, IContainer container, OrganizationTreeViewFactory organizationTreeViewFactory)
        {
            _DialogService = dialogService;
            _Container = container;
            _OrganizationTreeViewFactory = organizationTreeViewFactory;
        }

        public void Handle(SelectOrganizationRequest request)
        {
            using (var scope = _Container.BeginLifetimeScope())
            {
                var eventBus = scope.Resolve<IEventBus>();
                int? defaultOrgId = request.DefaultOrganizationId != null ? request.DefaultOrganizationId : default(int?);
                var view = scope.Resolve<OrganizationTreeViewFactory>().CreateOrganizationTreeViewFromRequest(request);

                _DialogService.ShowDialog<SelectOrganizationResponse>(view, request.Callback);
            }
        }
    }
}
