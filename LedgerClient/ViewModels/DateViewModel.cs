using LedgerClient.Infrastructure;

using System;
using System.Windows.Media;

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

        public override bool OkCanExecute() => Validator is null || Validator(Date);

        public DateViewModel()
        {
            Date = DateTime.Now;
            Validator = null;
        }
    }
}
