using System;
using System.Globalization;
using System.Windows.Data;

namespace MwoCWDropDeckBuilder.Infrastructure
{
    public class BoolToStringConverter : IValueConverter
    {
        public string TrueValue { get; set; }

        public string FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = string.Empty;
            bool? boolValue = value as bool?;
            if (boolValue.HasValue)
            {
                returnValue = (boolValue.Value) ? TrueValue : FalseValue;
            }
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Do nothing
            return null;
        }
    }
}