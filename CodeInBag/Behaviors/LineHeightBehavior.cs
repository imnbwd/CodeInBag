using System.Windows;
using System.Windows.Controls;

namespace CodeInBag.Behaviors
{
    public class LineHeightBehavior
    {
        public static readonly DependencyProperty MaxLinesProperty =
            DependencyProperty.RegisterAttached(
                "MaxLines",
                typeof(int),
                typeof(LineHeightBehavior),
                new PropertyMetadata(default(int), OnMaxLinesPropertyChangedCallback));

        public static int GetMaxLines(DependencyObject element)
        {
            return (int)element.GetValue(MaxLinesProperty);
        }

        public static void SetMaxLines(DependencyObject element, int value)
        {
            element.SetValue(MaxLinesProperty, value);
        }

        private static void OnMaxLinesPropertyChangedCallback(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var element = d as TextBlock;
            if (element != null)
            {
                element.MaxHeight = element.LineHeight * GetMaxLines(element);
            }
        }
    }
}