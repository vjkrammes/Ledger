using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LedgerClient.Controls;
using LedgerClient.Infrastructure;
using LedgerLib.Infrastructure;
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

        #region Utility Methods

        private string GetCopyrightFromAssembly()
        {
            var assem = GetType().Assembly;
            object[] attributes = assem.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true);
            if (attributes != null && attributes.Any())
            {
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
            return "Copyright information unavailable";
        }

        private string GetCompanyFromAssembly()
        {
            var assem = GetType().Assembly;
            object[] attributes = assem.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
            if (attributes != null && attributes.Any())
            {
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
            return "Company information unavailable";
        }

        #endregion

        public AboutViewModel(ISettingsService settings)
        {
            _settings = settings;
            ShortTitle = Tools.GetShortTitle();
            Credits = new ObservableDictionary<string, string>
            {
                ["System Id"] = _settings.SystemId.ToString(),
                ["Product"] = Constants.ProductName,
                ["Version"] = Constants.ProductVersion.ToString("n2"),
                ["Author"] = "V. James Krammes",
                ["Company"] = GetCompanyFromAssembly(),
                ["Copyright"] = GetCopyrightFromAssembly(),
                ["Platform"] = "Windows Desktop",
                ["Architecture"] = "Model - View - ViewModel (MVVM)",
                [".NET Version"] = "Microsoft .NET Core 3.0",
                ["Presentation"] = "Microsoft Windows Presentation Foundation (WPF)",
                ["Database"] = "Microsoft SQL",
                ["Database Access"] = "Microsoft Entity Framework Core",
                ["Entity Mapping"] = "AutoMapper by Jimmy Bogard",
                ["Text Handling"] = "Humanizer by Mehdi Khalili, Oren Novotny",
                ["Repository"] = "https://github.com/vjkrammes/Ledger"
            };
        }
    }
}
