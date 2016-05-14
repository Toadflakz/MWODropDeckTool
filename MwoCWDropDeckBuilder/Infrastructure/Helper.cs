using System;
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
