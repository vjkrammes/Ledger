using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using LedgerClient.Infrastructure;

namespace LedgerClient.ViewModels
{
    public class DateViewModel : ViewModelBase
    {
        #region Properties

        private string _prompt;
        public string Prompt
        {
            get => _prompt;
            set => SetProperty(ref _prompt, value);
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private SolidColorBrush _border;
        public SolidColorBrush Border
        {
            get => _border;
            set => SetProperty(ref _border, value);
        }

        public Func<DateTime?, bool> Validator { get; set; }

        #endregion

        public override bool OkCanExecute() => Validator is null ? true : Validator(Date);

        public DateViewModel()
        {
            Date = DateTime.Now;
            Validator = null;
        }
    }
}
