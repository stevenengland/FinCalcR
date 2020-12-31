using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Caliburn.Micro;

namespace StEn.FinCalcR.WinUi.Converter
{
    // Borrowed from https://spin.atomicobject.com/2015/10/19/caliburn-micro-design-time/
    public class DesignTimeViewModelLocator : IValueConverter
    {
        static DesignTimeViewModelLocator()
        {
            if (!Execute.InDesignMode)
            {
                return;
            }

            AssemblySource.Instance.Clear();
            AssemblySource.Instance.Add(typeof(DesignTimeViewModelLocator).Assembly);
        }

        public static DesignTimeViewModelLocator Instance { get; } = new DesignTimeViewModelLocator();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Blend creates types from a runtime/dynamic assembly, so match on name/namespace
            var viewModelType = typeof(DesignTimeViewModelLocator).Assembly.DefinedTypes
                .First(t =>
                {
                    var ns = value?.GetType().Namespace;
                    return t.Namespace != null && (ns != null && (t.Name == value?.GetType().Name && ns.EndsWith(t.Namespace)));
                }).AsType();

            return IoC.GetInstance(viewModelType, null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
