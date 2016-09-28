using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Controls;

namespace MetrologyAdmin
{
    public class BusyUmbrella: IBusyContent
    {
        public bool IsBusy
        {
            get 
            {
                if (_viewModels == null || _viewModels.Count() == 0)
                    return false;

                var result = false;

                Array.ForEach(_viewModels.ToArray(), vm => result = result || vm.IsBusy);

                return result;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private IList<IBusyContent> _viewModels = new List<IBusyContent>();

        public BusyUmbrella(params object[] busyContents)
        {
            foreach (var obj in busyContents)
            {
                var busyContent = obj as IBusyContent;
                if (busyContent == null)
                {
                    var control = obj as UserControl;
                    if (control != null)
                    {
                        busyContent = control.DataContext as IBusyContent;
                    }
                }
                if (busyContent != null)
                {
                    _viewModels.Add(busyContent);
                    busyContent.PropertyChanged += vm_PropertyChanged;
                }
            }
        }

        void vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBusy")
            {
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("propertyExpression should represent access to a member");
            var memberName = memberExpr.Member.Name;
            RaisePropertyChanged(memberName);
        }
    }
}
