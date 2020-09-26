using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace StEn.FinCalcR.Ui.Converter
{
	/// <summary>
	/// Universal converter. Borrowed from https://blog.scottlogic.com/2010/07/09/a-universal-value-converter-for-wpf.html .
	/// </summary>
	public class UniversalValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// obtain the conveter for the target type
			TypeConverter converter = TypeDescriptor.GetConverter(targetType);

			try
			{
				// determine if the supplied value is of a suitable type
				if (converter.CanConvertFrom(value.GetType()))
				{
					// return the converted value
					return converter.ConvertFrom(value);
				}
				else
				{
					// try to convert from the string representation
					return converter.ConvertFrom(value.ToString());
				}
			}
			catch (Exception)
			{
				return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
