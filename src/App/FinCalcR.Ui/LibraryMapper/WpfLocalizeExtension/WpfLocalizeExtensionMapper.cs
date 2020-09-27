using System.Globalization;
using System.Reflection;
using StEn.FinCalcR.Common.Services.Localization;
using WPFLocalizeExtension.Extensions;

namespace StEn.FinCalcR.Ui.LibraryMapper.WpfLocalizeExtension
{
	public class WpfLocalizeExtensionMapper : ILocalizationService
	{
		private readonly string resourcesIdentifier;

		public WpfLocalizeExtensionMapper(string resourcesIdentifier)
		{
			this.resourcesIdentifier = resourcesIdentifier;
		}

		public void ChangeCurrentCulture(CultureInfo cultureInfo)
		{
			WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
			WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = cultureInfo;
		}

		public T GetLocalizedValue<T>(string key)
		{
			return LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + $":{this.resourcesIdentifier}:" + key);
		}
	}
}
