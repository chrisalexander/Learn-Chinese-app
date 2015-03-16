using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LevelEditor.Converters
{
    /// <summary>
    /// Converts null to visibility.
    /// </summary>
    public class NullVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert from object to visibility.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The visibility.</returns>
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Convert from visibility to object. Not supported.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Not supported.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }
}
