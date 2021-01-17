using LedgerClient.Infrastructure;

using LedgerLib;
using LedgerLib.Entities;
using LedgerLib.Interfaces;

using System;

namespace LedgerClient.Models
{
    public class SettingsService : ISettingsService
    {
        private readonly SettingsEntity _settings;

        public SettingsService(LedgerContext context) => _settings = context.GetSettings;

        private void Persist() => Tools.Locator.SettingsDAL.Update(_settings);

        public Guid SystemId => _settings.SystemId;

        public string ProductName => _settings.ProductName;

        public double ProductVersion => _settings.ProductVersion;

        public string Alt0
        {
            get => _settings.Alt0;
            set
            {
                if (_settings.Alt0 != value)
                {
                    _settings.Alt0 = value;
                    Persist();
                }
            }
        }

        public string Alt1
        {
            get => _settings.Alt1;
            set
            {
                if (_settings.Alt1 != value)
                {
                    _settings.Alt1 = value;
                    Persist();
                }
            }
        }

        public string Background
        {
            get => _settings.Background;
            set
            {
                if (_settings.Background != value)
                {
                    _settings.Background = value;
                    Persist();
                }
            }
        }

        public string Border
        {
            get => _settings.Border;
            set
            {
                if (_settings.Border != value)
                {
                    _settings.Border = value;
                    Persist();
                }
            }
        }

        public string Foreground
        {
            get => _settings.Foreground;
            set
            {
                if (_settings.Foreground != value)
                {
                    _settings.Foreground = value;
                    Persist();
                }
            }
        }

        public double IconHeight
        {
            get => _settings.IconHeight;
            set
            {
                if (_settings.IconHeight != value)
                {
                    _settings.IconHeight = value;
                    Persist();
                }
            }
        }

        public bool ShowStatusbar
        {
            get => _settings.ShowStatusBar;
            set
            {
                if (_settings.ShowStatusBar != value)
                {
                    _settings.ShowStatusBar = value;
                    Persist();
                }
            }
        }

        public string BackupDirectory
        {
            get => _settings.BackupDirectory;
            set
            {
                if (_settings.BackupDirectory != value)
                {
                    _settings.BackupDirectory = value;
                    Persist();
                }
            }
        }

        public byte[] PasswordSalt
        {
            get => _settings.PasswordSalt;
            set
            {
                if (!_settings.PasswordSalt.ArrayEquals(value))
                {
                    _settings.PasswordSalt = value;
                    Persist();
                }
            }
        }

        public byte[] PasswordHash
        {
            get => _settings.PasswordHash;
            set
            {
                if (!_settings.PasswordHash.ArrayEquals(value))
                {
                    _settings.PasswordHash = value;
                    Persist();
                }
            }
        }

        public object this[string key]
        {
            get
            {
                var prop = _settings.GetType().GetProperty(key);
                if (prop is null)
                {
                    return null;
                }

                return prop.GetValue(_settings);
            }
        }
    }
}
