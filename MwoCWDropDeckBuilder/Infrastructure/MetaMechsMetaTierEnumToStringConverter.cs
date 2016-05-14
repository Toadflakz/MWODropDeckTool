using System;
using System.Globalization;
using System.Windows.Data;
using MwoCWDropDeckBuilder.Services.Interfaces;

namespace MwoCWDropDeckBuilder.Infrastructure
{
    public class MetaMechsMetaTierEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = string.Empty;
            try
            {
                var enumValue = (MetaMechsMetaTier) value;
                switch (enumValue)
                {
                    case MetaMechsMetaTier.Tier1:
                        returnValue = "Tier 1";
                        break;
                    case MetaMechsMetaTier.Tier2:
                        returnValue = "Tier 1 - 2";
                        break;
                    case MetaMechsMetaTier.Tier3:
                        returnValue = "Tier 1 - 3";
                        break;
                    case MetaMechsMetaTier.Tier4:
                        returnValue = "Tier 1 - 4";
                        break;
                    case MetaMechsMetaTier.Tier5:
                        returnValue = "Tier 1 - 5";
                        break;
                }
            }
            catch (Exception)
            {
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}