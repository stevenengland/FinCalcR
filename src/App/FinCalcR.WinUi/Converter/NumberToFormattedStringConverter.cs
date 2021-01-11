using System;
using System.Globalization;
using System.Windows.Data;

namespace StEn.FinCalcR.WinUi.Converter
{
    public class NumberToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "-";
            }

            if (!double.TryParse(value.ToString(), out var number))
            {
                return "-";
            }

            if (parameter == null)
            {
                parameter = 0;
            }

            var nfi = CultureInfo.CurrentUICulture.NumberFormat;
            nfi.NumberDecimalDigits = System.Convert.ToInt32(parameter);

            return number.ToString("N", nfi);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
    }
}
