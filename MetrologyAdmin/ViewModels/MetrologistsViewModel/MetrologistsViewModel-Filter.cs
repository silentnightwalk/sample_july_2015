using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MetrologyAdmin
{
    public partial class MetrologistsViewModel 
    {

        private void InitFilterCommands()
        {
            ApplyFilterCommand = new Command(CanExecute, ApplyFilterCommand_Execute);
            ClearFilterCommand = new Command((p) => true, ClearFilterCommand_Execute);
        }

        private bool CanExecute(object input)
        {
            return true;//Users != null && Users.Count > 0;
        }

        //-------------Filter-----------------------

        private bool _IsFilterOpen;
        public bool IsFilterOpen
        {
            get 
            { 
                return _IsFilterOpen; 
            }
            set 
            { 
                _IsFilterOpen = value;
                if (_IsFilterOpen == true)
                {
                    if (CurrentFilterValue != null)
                    {
                        FilterFio = CurrentFilterValue.Name;
                        FilterLogin = CurrentFilterValue.Login;
                        FilterOrganization = CurrentFilterValue.Organization;
                    }
                    else
                    {
                        FilterFio = null;
                        FilterLogin = null;
                        FilterOrganization = null;
                    }
                }
                PropertyChanged(this, new PropertyChangedEventArgs("IsFilterOpen"));
            }
        }
        

        private string _FilterFio = "";
        public string FilterFio
        {
            get { return _FilterFio; }
            set
            {
                _FilterFio = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FilterFio"));
            }
        }

        private string _FilterLogin = "";
        public string FilterLogin
        {
            get { return _FilterLogin; }
            set
            {
                _FilterLogin = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FilterLogin"));
            }
        }

        private string _FilterOrganization = "";
        public string FilterOrganization
        {
            get { return _FilterOrganization; }
            set
            {
                _FilterOrganization = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FilterOrganization"));
            }
        }


        private  FilterValue _CurrentFilterValue;
        public FilterValue CurrentFilterValue
        {
            get
            {
                return _CurrentFilterValue;
            }
            protected set
            {
                _CurrentFilterValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentFilterValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("IsFilterActive"));
            }
        }

        public bool IsFilterActive
        {
            get
            {
                return CurrentFilterValue != null;
            }
        }

        private void ApplyFilterCommand_Execute(object p = null)
        {
            IsFilterOpen = false;

            if(!(String.IsNullOrWhiteSpace(FilterFio) 
                && String.IsNullOrWhiteSpace(FilterLogin) 
                && String.IsNullOrWhiteSpace(FilterOrganization)
                ))
            {
                CurrentFilterValue = new FilterValue()
                {
                    Login = this.FilterLogin,
                    Name = this.FilterFio,
                    Organization = this.FilterOrganization
                };
            }
            else
            {
                CurrentFilterValue = null;
            }

            if (Users == null) return;

            var view = CollectionViewSource.GetDefaultView(Users);

            using (view.DeferRefresh())
            {
                if (CurrentFilterValue != null)
                {
                    //apply
                    view.Filter = FilterFunc;
                }
                else
                {
                    //reset
                    view.Filter = null;
                }
            }
        }

        private void ClearFilterCommand_Execute(object p)
        {
            IsFilterOpen = false;

            //TODO
            FilterFio = String.Empty;
            FilterLogin = String.Empty;
            FilterOrganization = String.Empty;
            CurrentFilterValue = null;

            if (Users != null && Users.Count() > 0)
            {
                var view = CollectionViewSource.GetDefaultView(Users);
                using (view.DeferRefresh())
                {
                    view.Filter = null;
                }
            };
        }

        protected bool FilterFunc(object item)
        {
            var target = item as UserViewModel;

            if (CurrentFilterValue == null) return true;

            if (target == null) return false;

            var filterFio = CurrentFilterValue.Name;
            var filterLogin = CurrentFilterValue.Login;
            var filterOrganization = CurrentFilterValue.Organization;
            
            var fioMatch = target.Name != null
                && (!String.IsNullOrWhiteSpace(filterFio)
                && target.Name.IndexOf(filterFio, StringComparison.OrdinalIgnoreCase) >= 0)
                || String.IsNullOrWhiteSpace(filterFio);

            var loginMatch = target.Login != null
                && (!String.IsNullOrWhiteSpace(filterLogin)
                && target.Login.IndexOf(filterLogin, StringComparison.OrdinalIgnoreCase) >= 0)
                || String.IsNullOrWhiteSpace(filterLogin);

            var orgMatch = target.SubDivisionName != null
                && (!String.IsNullOrWhiteSpace(filterOrganization)
                && target.SubDivisionName.IndexOf(filterOrganization, StringComparison.OrdinalIgnoreCase) >= 0)
                || String.IsNullOrWhiteSpace(filterOrganization);

            return fioMatch && loginMatch && orgMatch;
        }


        public class FilterValue
        {
            public string Name { get; set; }
            public string Login { get; set; }
            public string Organization { get; set; }

            public override string ToString()
            {
                var criterias = new List<string>();

                if(!String.IsNullOrWhiteSpace(Name))
                    criterias.Add(String.Format("Ф.И.О. содержит \"{0}\"", Name));

                if (!String.IsNullOrWhiteSpace(Login))
                    criterias.Add(String.Format("логин содержит \"{0}\"", Login));

                if (!String.IsNullOrWhiteSpace(Organization))
                    criterias.Add(String.Format("подразделение содержит \"{0}\"", Organization));

                var result = String.Join(";  ", criterias);

                return result;
            }
        }
        
    }
}
