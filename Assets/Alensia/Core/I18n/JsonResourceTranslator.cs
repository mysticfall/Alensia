using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public class JsonResourceTranslator : ResourceTranslator<TextAsset>
    {
        public JsonResourceTranslator(ResourceSettings resourceSettings, ILocaleService localeService) :
            base(resourceSettings, localeService)
        {
        }

        protected override ITranslationSet Load(TextAsset resource, CultureInfo locale)
        {
            Assert.IsNotNull(resource, "resource != null");

            var dictionary = JObject
                .Parse(resource.text)
                .Descendants()
                .Where(p => !p.Any())
                .Aggregate(new Dictionary<string, string>(), Aggregate);

            return new DictionaryTranslationSet(dictionary);
        }

        private static Dictionary<string, string> Aggregate(
            Dictionary<string, string> source, JToken token)
        {
            source.Add(token.Path, token.ToString());

            return source;
        }
    }
}