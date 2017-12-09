using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Alensia.Core.Common;
using Malee;
using UnityEngine;

namespace Alensia.Core.I18n
{
    public abstract class ResourceTranslator<T> : Translator where T : Object
    {
        public IEnumerable<string> ResourceNames => _resources;

        [SerializeField, Reorderable] private StringList _resources;

        protected ResourceTranslator()
        {
            _resources = new StringList {"Translations/Messages"};
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