using System;
using System.ComponentModel.DataAnnotations;

using LedgerLib.Infrastructure;

namespace LedgerLib.Entities
{
    public class SettingsEntity
    {
        [Required]
        public Guid SystemId { get; set; }
        [Required, MaxLength(50)]
        public string ProductName { get; set; }
        [Required, Positive]
        public double ProductVersion { get; set; }
        [Required, MaxLength(50)]
        public string Alt0 { get; set; }
        [Required, MaxLength(50)]
        public string Alt1 { get; set; }
        [Required, MaxLength(50)]
        public string Background { get; set; }
        [Required, MaxLength(50)]
        public string Border { get; set; }
        [Required, MaxLength(50)]
        public string Foreground { get; set; }
        [Required, Range(Constants.MinumumIconHeight, Constants.MaximumIconHeight)]
        public double IconHeight { get; set; }
        [Required]
        public bool ShowStatusBar { get; set; }
        [Required]
        public string BackupDirectory { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public static SettingsEntity Default
        {
            get => new SettingsEntity
            {
                SystemId = Guid.NewGuid(),
                ProductName = Constants.ProductName,
                ProductVersion = Constants.ProductVersion,
                Alt0 = "AliceBlue",
                Alt1 = "FloralWhite",
                Background = "DarkSlateGray",
                Border = "Black",
                Foreground = "White",
                IconHeight = Constants.DefaultIconHeight,
                ShowStatusBar = true,
                BackupDirectory = string.Empty,
                PasswordSalt = null,
                PasswordHash = null
            };
        }
    }
}
