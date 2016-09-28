using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public partial class MetrologistsViewModel
    {

        private bool OrganizationsUsersShouldBeOnTheList(int serverId, int organizationId)
        {
            if (AggregateOn)
            {
                var orgTree = _readModel.GetOrganizationsTree(serverId);
                var targetGrandChild = Organization
                    .AsEnumerable(new Organization[] { SelectedOrganization })
                    .FirstOrDefault(x => x.Id == organizationId);
                if (targetGrandChild != null) return true;
                else return false;
            }
            else
            {
                return organizationId == SelectedOrganization.Id; 
            }
        }

        private UserViewModel GetUserVm(int serverId, int targetUserId)
        {
            var targetUser = _readModel.GetUserById(serverId, targetUserId);
            return new UserViewModel(targetUser);
        }

        private UserViewModel GetUserVm(int serverId, string login, string password)
        {
            var targetUser = _readModel.GetUserByLoginDetails(serverId, login, password);
            return new UserViewModel(targetUser);
        }

        //---------------------------------------------

        private void ProcessAddAction(int serverId, string login, string password)
        {
            var newUserVm = GetUserVm(serverId, login, password);
            var organizationsUsersShouldBeOnTheList = OrganizationsUsersShouldBeOnTheList(serverId, newUserVm.OrganizationId.Value);
            if (organizationsUsersShouldBeOnTheList)
                AddUserFromLocalList_v2(newUserVm);
        }

        private void ProcessEditAction(int serverId, int userId)
        {
            var targetUserVm = GetUserVm(serverId, userId);
            var organizationsUsersShouldBeOnTheList = OrganizationsUsersShouldBeOnTheList(serverId, targetUserVm.OrganizationId.Value);
            if (organizationsUsersShouldBeOnTheList)
                EditUserInLocalList_v2(targetUserVm);
            else
                RemoveUserFromLocalList_v2(targetUserVm.Id);
        }

        private void ProcessDeleteAction(int serverId, int userId)
        {
            RemoveUserFromLocalList_v2(userId);
        }

        //-------------------------------------------

        private void AddUserFromLocalList_v2(UserViewModel user)
        {
            Users.Add(user);
            RenewComandsCanExecute();
        }

        private void EditUserInLocalList_v2(UserViewModel user)
        {
            var target = Users.SingleOrDefault(x => x.Id == user.Id);
            if (target != null)
            {
                var targetIndex = Users.IndexOf(target);
                Users[targetIndex] = user;
                RenewComandsCanExecute();
            }
        }

        private void RemoveUserFromLocalList_v2(int userId)
        {
            var target = Users.SingleOrDefault(x => x.Id == userId);
            if (target != null)
            {
                var targetIndex = Users.IndexOf(target);
                Users.RemoveAt(targetIndex);
                RenewComandsCanExecute();
            }
        }

        //------------------------------------------

        private void RenewComandsCanExecute()
        {
            this.DeleteCommand.RaiseCanExecuteChanged();
            this.EditCommand.RaiseCanExecuteChanged();
            this.EmailCommand.RaiseCanExecuteChanged();
            this.ReportCommand.RaiseCanExecuteChanged();
        }
    }
}
