using System.Windows.Controls;

namespace LedgerClient.Controls
{
    public class ScrollListBox : ListBox
    {
        public ScrollListBox() : base() => SelectionChanged += Scroll;

        private void Scroll(object sender, SelectionChangedEventArgs e)
        {
            if (e?.AddedItems != null && e.AddedItems.Count > 0)
            {
                ScrollIntoView(e.AddedItems[0]);
            }
        }
    }
}
