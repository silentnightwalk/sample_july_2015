using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MetrologyAdmin
{
    public partial class App : Application
    {
        static App()
        {
            TextOptions.TextFormattingModeProperty.OverrideMetadata(
                typeof(UIElement),
                new FrameworkPropertyMetadata(TextFormattingMode.Display)
                );
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run(); // true - real world database // false - fake database
        }
    }
}
