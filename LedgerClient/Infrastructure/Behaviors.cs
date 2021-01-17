using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LedgerClient.Infrastructure
{
    public class Behaviors
    {

        // Drop

        public static readonly DependencyProperty DropBehaviourProperty =
                DependencyProperty.RegisterAttached("DropBehaviour", typeof(ICommand),
                typeof(Behaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                OnDropBehaviourChanged));

        public static ICommand GetDropBehaviour(DependencyObject d) => (ICommand)d.GetValue(DropBehaviourProperty);
        public static void SetDropBehaviour(DependencyObject d, ICommand value) =>
            d.SetValue(DropBehaviourProperty, value);
        public static void OnDropBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                element.Drop += (s, a) =>
                {
                    var command = GetDropBehaviour(element);
                    if (command != null)
                    {
                        if (command.CanExecute(a.Data))
                        {
                            command.Execute(a.Data);
                        }
                    }
                };
            }
        }

        // MouseLeftButtonUp

        public static readonly DependencyProperty MouseLeftButtonUpBehaviourProperty =
                DependencyProperty.RegisterAttached("MouseLeftButtonUpBehaviour",
                typeof(ICommand), typeof(Behaviors), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.None, OnMouseLeftButtonUpPropertyChanged));

        public static ICommand GetMouseLeftButtonUpBehaviour(DependencyObject d) =>
            (ICommand)d.GetValue(MouseLeftButtonUpBehaviourProperty);
        public static void SetMouseLeftButtonUpBehaviour(DependencyObject d, ICommand value) =>
            d.SetValue(MouseLeftButtonUpBehaviourProperty, value);
        public static void OnMouseLeftButtonUpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                element.MouseLeftButtonUp += (s, a) =>
                {
                    var command = GetMouseLeftButtonUpBehaviour(element);
                    if (command != null)
                    {
                        if (command.CanExecute(a))
                        {
                            command.Execute(a);
                        }
                    }
                };
            }
        }

        // MouseDoubleClick

        public static readonly DependencyProperty MouseDoubleClickBehaviourProperty =
                DependencyProperty.RegisterAttached("MouseDoubleClickBehaviour",
                typeof(ICommand), typeof(Behaviors), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.None, OnMouseDoubleClickBehaviourChanged));

        public static ICommand GetMouseDoubleClickBehaviour(DependencyObject d) =>
            (ICommand)d.GetValue(MouseDoubleClickBehaviourProperty);
        public static void SetMouseDoubleClickBehaviour(DependencyObject d, ICommand value) =>
            d.SetValue(MouseDoubleClickBehaviourProperty, value);
        public static void OnMouseDoubleClickBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control)
            {
                control.MouseDoubleClick += (s, a) =>
                {
                    var command = GetMouseDoubleClickBehaviour(control);
                    if (command != null)
                    {
                        if (command.CanExecute(a))
                        {
                            command.Execute(a);
                        }
                    }
                };
            }
        }

        // Window Loaded

        public static readonly DependencyProperty WindowLoadedBehaviourProperty =
                DependencyProperty.RegisterAttached("WindowLoadedBehaviour", typeof(ICommand),
                typeof(Behaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                OnWindowLoadedBehaviourChanged));

        public static ICommand GetWindowLoadedBehaviour(DependencyObject d) =>
            (ICommand)d.GetValue(WindowLoadedBehaviourProperty);
        public static void SetWindowLoadedBehaviour(DependencyObject d, ICommand value) =>
            d.SetValue(WindowLoadedBehaviourProperty, value);
        public static void OnWindowLoadedBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window w)
            {
                w.Loaded += (s, a) =>
                {
                    var command = GetWindowLoadedBehaviour(w);
                    if (command != null)
                    {
                        if (command.CanExecute(a))
                        {
                            command.Execute(a);
                        }
                    }
                };
            }
        }

        // Window Closed

        public static readonly DependencyProperty WindowClosedBehaviorProperty =
                DependencyProperty.RegisterAttached("WindowClosedBehaviour", typeof(ICommand),
                typeof(Behaviors), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None,
                OnWindowClosedBehaviourChanged));

        public static ICommand GetWindowClosedBehaviour(DependencyObject d) =>
            (ICommand)d.GetValue(WindowClosedBehaviorProperty);
        public static void SetWindowClosedBehaviour(DependencyObject d, ICommand value) =>
            d.SetValue(WindowClosedBehaviorProperty, value);
        public static void OnWindowClosedBehaviourChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window w)
            {
                w.Closed += (s, a) =>
                {
                    var command = GetWindowClosedBehaviour(w);
                    if (command != null)
                    {
                        if (command.CanExecute(a))
                        {
                            command.Execute(a);
                        }
                    }
                };
            }
        }
    }
}
