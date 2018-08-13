using System.Windows;
using FriendOrganizer.Core.Services;

namespace FriendOrganizer.UI.View.Services
{
    public sealed class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK
                ? MessageDialogResult.Ok
                : MessageDialogResult.Cancel;
        }

        public void ShowInfoDialog(string text)
        {
            MessageBox.Show(text, "Info");
        }
    }
}