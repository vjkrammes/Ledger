using System.Windows.Controls;
using System.Windows.Input;

using LedgerClient.Infrastructure;

namespace LedgerClient.Models
{
    public class MenuInfo : NotifyBase
    {
        private string _header;
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        private object _tag;
        public object Tag
        {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }

        private Image _icon;
        public Image Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private ICommand _command;
        public ICommand Command
        {
            get => _command;
            set => SetProperty(ref _command, value);
        }

        private object _commandParameter;
        public object CommandParameter
        {
            get => _commandParameter;
            set => SetProperty(ref _commandParameter, value);
        }

        public override string ToString() => Header;
    }
}
