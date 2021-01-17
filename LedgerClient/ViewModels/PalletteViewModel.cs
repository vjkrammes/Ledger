using LedgerClient.Infrastructure;

using LedgerLib.Interfaces;

using System.Collections.Generic;
using System.Windows.Input;

namespace LedgerClient.ViewModels
{
    public class PalletteViewModel : ViewModelBase
    {

        #region Properties

        private List<string> _colorNames;
        public List<string> ColorNames
        {
            get => _colorNames;
            set => SetProperty(ref _colorNames, value);
        }

        private string _borderColor;
        public string BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }

        private string _foregroundColor;
        public string ForegroundColor
        {
            get => _foregroundColor;
            set => SetProperty(ref _foregroundColor, value);
        }

        private string _alt0Color;
        public string Alt0Color
        {
            get => _alt0Color;
            set => SetProperty(ref _alt0Color, value);
        }

        private string _alt1Color;
        public string Alt1Color
        {
            get => _alt1Color;
            set => SetProperty(ref _alt1Color, value);
        }

        private readonly ISettingsService _settings;

        #endregion

        #region Commands

        private RelayCommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new RelayCommand(param => ResetClick(), param => AlwaysCanExecute());
                }
                return _resetCommand;
            }
        }

        #endregion

        #region Command Methods

        public override void OK()
        {
            _settings.Border = BorderColor;
            _settings.Background = BackgroundColor;
            _settings.Foreground = ForegroundColor;
            _settings.Alt0 = Alt0Color;
            _settings.Alt1 = Alt1Color;
            Pallette.SetSystemColors();
            DialogResult = true;
        }

        public void ResetClick()
        {
            BorderColor = "Black";
            BackgroundColor = "DarkSlateGray";
            ForegroundColor = "White";
            Alt0Color = "AliceBlue";
            Alt1Color = "FloralWhite";
        }

        #endregion

        public PalletteViewModel(ISettingsService settings)
        {
            _settings = settings;
            ColorNames = Pallette.Names();
            BorderColor = _settings.Border;
            BackgroundColor = _settings.Background;
            ForegroundColor = _settings.Foreground;
            Alt0Color = _settings.Alt0;
            Alt1Color = _settings.Alt1;
        }
    }
}
