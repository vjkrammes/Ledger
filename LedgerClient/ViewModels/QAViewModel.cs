using System;
using System.Windows;
using System.Windows.Media;

using LedgerClient.Infrastructure;

using LedgerLib.Infrastructure;

namespace LedgerClient.ViewModels
{
    public class QAViewModel : ViewModelBase
    {
        #region Properties

        private string _question;
        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        private string _answer;
        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        private bool _answerRequired;
        public bool AnswerRequired
        {
            get => _answerRequired;
            set => SetProperty(ref _answerRequired, value);
        }

        private SolidColorBrush _borderBrush;
        public SolidColorBrush BorderBrush
        {
            get => _borderBrush;
            set => SetProperty(ref _borderBrush, value);
        }

        public Func<string, bool> Validator { get; set; }

        #endregion

        #region Command Methods

        public override bool OkCanExecute()
        {
            if (AnswerRequired && string.IsNullOrEmpty(Answer))
            {
                return false; 
            }
            if (!AnswerRequired && string.IsNullOrEmpty(Answer) && Validator is null)
            {
                return true;
            }
            if (Validator is null)
            {
                return true;
            }
            return Validator(Answer);
        }

        #endregion

        public QAViewModel()
        {
            BorderBrush = Application.Current.Resources[Constants.Border] as SolidColorBrush ?? Brushes.Black;
            Validator = null;
        }
    }
}
