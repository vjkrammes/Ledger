using System;
using System.Collections.Generic;
using System.Text;
using LedgerClient.Controls;
using LedgerClient.Infrastructure;
using LedgerLib.Interfaces;

namespace LedgerClient.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        #region Properties

        private ObservableDictionary<string, string> _credits;
        public ObservableDictionary<string, string> Credits
        {
            get => _credits;
            set => SetProperty(ref _credits, value);
        }

        // note - settings does not need to be observable since any properties bound from it are one-way

        private ISettingsService _settings;
        public ISettingsService Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        private string _shortTitle;
        public string ShortTitle
        {
            get => _shortTitle;
            set => SetProperty(ref _shortTitle, value);
        }

        #endregion

        public AboutViewModel(ISettingsService settings)
        {
            _settings = settings;
            ShortTitle = Tools.GetShortTitle(_settings);
        }
    }
}
