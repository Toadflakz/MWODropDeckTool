using System.Windows;

namespace MwoCWDropDeckBuilder.Infrastructure.Interfaces
{
    public interface IInteractionService
    {
        string GetSaveFileFromUser(string filter);
        string GetOpenFileFromUser(string filter);

        MessageBoxResult ShowMessageBox(string title, string caption, MessageBoxButton messageBoxButton, MessageBoxImage image);
    }
}