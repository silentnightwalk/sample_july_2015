using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public interface IBusyContent: INotifyPropertyChanged
    {
        bool IsBusy { get; set; }
    }
}
