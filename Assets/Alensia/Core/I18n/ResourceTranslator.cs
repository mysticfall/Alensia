using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public abstract class ResourceTranslator<T> : Translator where T : Object
    {
        public string BaseName => _resourceSettings.BaseName;

        public string ResourcePath => _resourceSettings.ResourcePath ?? "";

        private readonly ResourceSettings _resourceSettings;

        protected ResourceTranslator(
            ResourceSettings resourceSettings, ILocaleService localeService) : base(localeService)
        {
            Assert.IsNotNull(resourceSettings, "resourceSettings != null");
            Assert.IsNotNull(resourceSettings.BaseName, "resourceSettings.BaseName != null");

            _resourceSettings = resourceSettings;
        }

        protected virtual string GetResourceName(CultureInfo locale)
        {
            var path = ResourcePath.Length == 0 || ResourcePath.EndsWith("/")
                ? ResourcePath
                : ResourcePath + "/";

            return $"{path}{BaseName}-{locale.Name}";
        }

        protected override ITranslationSet Load(CultureInfo locale)
        {
            var resource = Resources.Load<T>(GetResourceName(locale));

            return resource == null ? null : Load(resource, locale);
        }

        protected abstract ITranslationSet Load(T resource, CultureInfo locale);
    }
}