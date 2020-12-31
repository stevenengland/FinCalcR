using System.Globalization;

namespace StEn.FinCalcR.Common.Services.Localization
{
    public interface ILocalizationService
    {
        void ChangeCurrentCulture(CultureInfo cultureInfo);

        T GetLocalizedValue<T>(string key);
    }
}
