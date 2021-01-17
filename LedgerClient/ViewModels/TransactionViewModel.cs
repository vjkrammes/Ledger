using LedgerClient.Infrastructure;

using System;

namespace LedgerClient.ViewModels
{
    public class TransactionViewModel : ViewModelBase
    {
        #region Properties

        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        private decimal _payment;
        public decimal Payment
        {
            get => _payment;
            set => SetProperty(ref _payment, value);
        }

        private string _reference;
        public string Reference
        {
            get => _reference;
            set => SetProperty(ref _reference, value);
        }

        #endregion

        #region Command Methods

        public override bool OkCanExecute() => Balance != 0 && Payment != 0 && Payment <= Balance && Date != default;

        #endregion

        public TransactionViewModel() => Date = DateTime.Now;
    }
}
