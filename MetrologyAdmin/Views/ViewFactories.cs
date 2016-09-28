using MetrologyAdmin.ApplicationLayer;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using MetrologyAdmin.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class AddEditUserViewFactory
    {
        private readonly IEventBus _eventBus;
        private readonly IRequestBus _requestBus;
        private readonly IReadModelFacade _readModel;
        private readonly SettingsManager _settingsManager;
        private readonly IUserService _userService;
        private readonly EmailService _emailService;

        public AddEditUserViewFactory(IEventBus eventBus, IRequestBus requestBus, IReadModelFacade readModel, SettingsManager settingsManager, IUserService userService, EmailService emailService)
        {
            _eventBus = eventBus;
            _requestBus = requestBus;
            _readModel = readModel;
            _settingsManager = settingsManager;
            _userService = userService;
            _emailService = emailService;
        }

        public AddEditUserView CreateAddUserView(AddUserRequest request)
        {
            var viewModel = new AddUserViewModel(request, _eventBus, _requestBus, _readModel, _settingsManager, _userService, _emailService);
            return new AddEditUserView(viewModel);
        }

        public AddEditUserView CreateEditUserView(EditUserRequest request)
        {
            var viewModel = new EditUserViewModel(request, _eventBus, _requestBus, _readModel, _settingsManager, _userService, _emailService);
            return new AddEditUserView(viewModel);
        }
    }


    public class OrganizationTreeViewFactory
    {
        private readonly IReadModelFacade _ReadModelFacade;
        private readonly IEventBus _EventBus;
        private readonly SettingsManager _SettingsManager;

        public OrganizationTreeViewFactory(IReadModelFacade readModelFacade, IEventBus eventBus, SettingsManager settingsManager)
        {
            _ReadModelFacade = readModelFacade;
            _EventBus = eventBus;
            _SettingsManager = settingsManager;
        }

        public OrganizationTreeView CreateOrganizationTreeView(int? defaultOrganizationId = null, int? serverId = null)
        {
            var viewModel = new OrganizationsTreeViewModel(_ReadModelFacade, _EventBus, _SettingsManager, defaultOrganizationId, serverId);
            return new OrganizationTreeView(viewModel);
        }

        public OrganizationTreeView CreateOrganizationTreeViewFromRequest(SelectOrganizationRequest request)
        {
            return CreateOrganizationTreeView(request.DefaultOrganizationId, request.ServerId);
        }
    }
}
