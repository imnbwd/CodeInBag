using System.Windows;
using System.Windows.Input;

namespace CodeInBag.Behaviors
{
    public static class MouseDoubleClickBehavior
    {
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(MouseDoubleClickBehavior), new FrameworkPropertyMetadata((ICommand)null));
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(MouseDoubleClickBehavior), new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnHandleDoubleClickCommandChanged)));

        public static object GetCommandParameter(DependencyObject d)
        {
            return d.GetValue(CommandParameterProperty);
        }

        public static ICommand GetDoubleClickCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(DoubleClickCommandProperty);
        }

        public static void SetCommandParameter(DependencyObject d, object value)
        {
            d.SetValue(CommandParameterProperty, value);
        }

        public static void SetDoubleClickCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(DoubleClickCommandProperty, value);
        }

        private static void OnHandleDoubleClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement ele = d as FrameworkElement;
            if (ele != null)
            {
                ele.MouseLeftButtonDown -= OnMouseDoubleClick;
                if (e.NewValue != null)
                {
                    ele.MouseLeftButtonDown += OnMouseDoubleClick;
                }
            }
        }

        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;
            FrameworkElement ele = sender as FrameworkElement;

            DependencyObject originalSender = e.OriginalSource as DependencyObject;
            if (ele == null || originalSender == null)
                return;

            ICommand command = (ICommand)(sender as DependencyObject).GetValue(DoubleClickCommandProperty);
            var parameter = (sender as DependencyObject).GetValue(CommandParameterProperty);
            if (command != null)
            {
                if (command.CanExecute(null))
                    command.Execute(parameter);
            }
        }
    }
}