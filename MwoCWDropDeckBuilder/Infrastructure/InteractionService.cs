using System.Windows;
using Microsoft.Win32;
using MwoCWDropDeckBuilder.Infrastructure.Interfaces;

namespace MwoCWDropDeckBuilder.Infrastructure
{
    public class InteractionService : IInteractionService
    {
        public InteractionService()
        {
        }

        public string GetSaveFileFromUser(string filter)
        {
            string returnValue = string.Empty;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = filter;
            var result = dialog.ShowDialog(Application.Current.MainWindow);
            if (result.HasValue && result == true)
                returnValue = dialog.FileName;
            return returnValue;
        }

        public string GetOpenFileFromUser(string filter)
        {
            string returnValue = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            var result = dialog.ShowDialog(Application.Current.MainWindow);
            if (result.HasValue && result == true)
                returnValue = dialog.FileName;
            return returnValue;
        }


        public MessageBoxResult ShowMessageBox(string title, string caption, MessageBoxButton messageBoxButton, MessageBoxImage image)
        {
            return MessageBox.Show(caption, title, messageBoxButton, image);
        }
    }
}