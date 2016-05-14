using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MwoCWDropDeckBuilder.Infrastructure
{
    public static class Helper
    {

        public static void InvokeForUI(Action action)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                action();
            }, DispatcherPriority.Send);
        }

    }
}
