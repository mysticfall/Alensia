using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public abstract class ResourceTranslator<T> : Translator where T : Object
    {
        public IReadOnlyList<string> ResourceNames => _resourceSettings.Resources;

        private readonly ResourceSettings _resourceSettings;

        protected ResourceTranslator(
            ResourceSettings resourceSettings, ILocaleService localeService) : base(localeService)
        {
            Assert.IsNotNull(resourceSettings, "resourceSettings != null");
            Assert.IsNotNull(resourceSettings.Resources, "resourceSettings.Resources == null");

            _resourceSettings = resourceSettings;
        }

        protected virtual string GetResourceName(string baseName, CultureInfo locale) =>
            $"{baseName}-{locale.Name}";

        protected override IMessages Load(CultureInfo locale, IMessages parent)
        {
            var resources = ResourceNames
                .Select(r => GetResourceName(r, locale))
                .SelectMany(Resources.LoadAll<T>)
                .ToList();


            return Load(resources, locale, parent);
        }

        protected abstract IMessages Load(IReadOnlyList<T> resources, CultureInfo locale, IMessages parent);
    }
}