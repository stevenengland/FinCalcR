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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3963:\"static\" fields should be initialized inline", Justification = "Cloned from website")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Justification = "Cloned from website")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "Cloned from website")]
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
