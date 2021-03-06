﻿using System.Globalization;
using System.Reflection;
using StEn.FinCalcR.Common.Services.Localization;
using WPFLocalizeExtension.Extensions;

namespace StEn.FinCalcR.WinUi.LibraryMapper.WpfLocalizeExtension
{
    public class WpfLocalizeExtensionMapper : ILocalizationService
    {
        private readonly string resourcesIdentifier;

        public WpfLocalizeExtensionMapper(string resourcesIdentifier)
        {
            this.resourcesIdentifier = resourcesIdentifier;
        }

        /// <summary>
        /// Sets the culture, thread and threadUI culture at once.
        /// </summary>
        /// <param name="cultureInfo">A valid culture info that is supported.</param>
        public void ChangeCurrentCulture(CultureInfo cultureInfo)
        {
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true; // Threads culture and UI culture will be set along with the culture itself.
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = cultureInfo;
        }

        public T GetLocalizedValue<T>(string key) => LocExtension.GetLocalizedValue<T>(Assembly.GetCallingAssembly().GetName().Name + $":{this.resourcesIdentifier}:" + key);
    }
}
