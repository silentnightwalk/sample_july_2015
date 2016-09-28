using Cniitei.MVVM;
using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MetrologyAdmin
{
    public partial class OrganizationsTreeViewModel
    {
        private string _SerachQuery;
        public string SearchQuery
        {
            get { return _SerachQuery; }
            set
            {
                if (SetValue(ref _SerachQuery, value, "SearchQuery"))
                {
                    ApplyFilter(_SerachQuery);
                }
            }
        }

        private void ApplyFilter(string query)
        {
            IsFilterActive = !String.IsNullOrWhiteSpace(query);

            if (IsFilterActive)
            {
                int matchedItemCount = 0;
                _SearchResult = Search(query, out matchedItemCount);
                MatchedItemsCount = matchedItemCount;
            }
            else
            {
                _SearchResult = null;
            }
            PropertyChanged(this, new PropertyChangedEventArgs("OrganizationTree"));
        }

        private IEnumerable<OrganizationViewModel> Search(string query, out int matchedCount)
        {
            var matches = OrganizationViewModel.AsEnumerable(_Source)
                .Where(x => x.Name.IndexOf(query, StringComparison.InvariantCultureIgnoreCase) >= 0)
                .ToArray();

            matchedCount = matches.Length;

            var matchedIds = matches.Select(x => x.Id).ToArray();

            var result = matches
                .Union(matches.SelectMany(x => OrganizationViewModel.Ancestors(x)))
                .Distinct()
                .Select(x => x.CloneWithoutChildren())
                .ToArray();

            List<OrganizationViewModel> searchResultRoots = new List<OrganizationViewModel>();

            foreach (var group in result.GroupBy(x => x.ParentId))
            {
                var parent = result.FirstOrDefault(x => x.Id == group.Key);
                if (parent == null)
                {
                    searchResultRoots.AddRange(group);
                }
                else
                {
                    parent.IsExpanded = true;
                    parent.Children = group.ToArray();
                    foreach (var child in group)
                    {
                        child.Parent = parent;
                        child.IsMatch = matchedIds.Contains(child.Id);
                    }
                }
            }

            return searchResultRoots;
        }

        private bool _IsFilterActive;
        public bool IsFilterActive
        {
            get { return _IsFilterActive; }
            set { SetValue(ref _IsFilterActive, value, "IsFilterActive"); }
        }

        private int _MatchedItemsCount;
        public int MatchedItemsCount
        {
            get { return _MatchedItemsCount; }
            set { SetValue(ref _MatchedItemsCount, value, "MatchedItemsCount"); }
        }



        //private Organization[] _OrganizationTree;
        //public Organization[] OrganizationTree
        //{
        //    get { return _OrganizationTree; }
        //    set
        //    {
        //        _OrganizationTree = value;
        //        if (_OrganizationTree != null)
        //            Array.ForEach(_OrganizationTree, x => ((OrganizationViewModel)x).IsExpanded = true);
        //        PropertyChanged(this, new PropertyChangedEventArgs("OrganizationTree"));
        //    }
        //}


        private IEnumerable<OrganizationViewModel> _SearchResult;

        private IEnumerable<OrganizationViewModel> _Source;
        public IEnumerable<OrganizationViewModel> OrganizationTree
        {
            get
            {
                return _SearchResult ?? _Source;
            }
            protected set
            {
                _Source = value;
                if (_Source != null)
                {
                    foreach(var x in _Source)
                    {
                        x.IsExpanded = true;
                    }
                }
                PropertyChanged(this, new PropertyChangedEventArgs("OrganizationTree"));
            }
        }

        protected bool SetValue<T>(ref T backingField, T value, string propertyName)
        {
            if ((backingField == null && value != null) || (backingField != null && !backingField.Equals(value)))
            {
                backingField = value;
                PropertyChanged(this, new PropertyChangedEventArgs("propertyName"));
                return true;
            }
            return false;
        }
    }
}
