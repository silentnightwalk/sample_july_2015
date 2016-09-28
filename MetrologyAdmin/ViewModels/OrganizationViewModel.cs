using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class OrganizationViewModel: Organization, INotifyPropertyChanged
    {
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set 
            { 
                _IsSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set 
            { 
                _IsExpanded = value;
                PropertyChanged(this,new PropertyChangedEventArgs("IsExpanded"));
            }
        }

        private bool _IsMatch;
        public bool IsMatch
        {
            get { return _IsMatch; }
            set
            {
                _IsMatch = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsMatch"));
            }
        }

        private OrganizationViewModel()
        {

        }

        private OrganizationViewModel(Organization baseOrganization, OrganizationViewModel parent = null, bool withoutChildren = false)
        {
            if (!withoutChildren)
            {
                this.Children = baseOrganization.Children != null
                              ? baseOrganization.Children.Select(org => new OrganizationViewModel(org, this)).ToArray()
                              : null;
            }
            else
            {
                this.IsSelected = false;
                this.Children = Enumerable.Empty<OrganizationViewModel>().ToArray();
            }

            this.Id = baseOrganization.Id;
            this.Name = baseOrganization.Name;
            this.NodGuid = baseOrganization.NodGuid;
            this.ParentId = baseOrganization.ParentId;
            this.RowGuid = baseOrganization.RowGuid;
            this.Parent = parent;
            this.DivisionName = baseOrganization.DivisionName;
            this.FilialName = baseOrganization.FilialName;
            this.SubdivisionName = baseOrganization.SubdivisionName;

            //Debug.WriteLine(String.Format("OVM constructor. Name: {0}, Hash: {1}",this.Name, this.GetHashCode()));
        }

        public OrganizationViewModel(Organization baseOrganization, bool withoutChildren = false)
            : this(baseOrganization, null, withoutChildren)
        {

        }

        public static IEnumerable<OrganizationViewModel> AsEnumerable(IEnumerable<OrganizationViewModel> roots)
        {
            if (roots == null) yield break;

            foreach (var item in roots)
            {
                yield return item;
                foreach (var child in AsEnumerable(item.Children))
                {
                    var childVM = child as OrganizationViewModel;
                    if (childVM != null) yield return childVM;
                }
            }
        }

        public static IEnumerable<OrganizationViewModel> Ancestors(OrganizationViewModel item)
        {
            var parent = item.Parent;
            while (parent != null)
            {
                var parentVM = parent as OrganizationViewModel;
                if (parentVM != null) yield return parentVM;
                parent = parent.Parent;
            }
        }

        public OrganizationViewModel CloneWithoutChildren()
        {
            //var result = (Organization)this.MemberwiseClone();
            //result.Children = Enumerable.Empty<Organization>().ToArray();
            return new OrganizationViewModel(this,true);
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
