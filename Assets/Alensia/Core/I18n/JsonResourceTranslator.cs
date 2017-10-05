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
        protected override IMessages Load(
            IReadOnlyList<TextAsset> resources, CultureInfo locale, IMessages parent)
        {
            Assert.IsNotNull(resources, "resource != null");
            Assert.IsNotNull(locale, "locale != null");

            var dictionary = resources
                .Select(r => r.text)
                .Select(JObject.Parse)
                .Descendants()
                .Where(p => !p.Any())
                .Aggregate(new Dictionary<string, string>(), Aggregate);

            return new DictionaryMessages(dictionary, parent);
        }

        private static Dictionary<string, string> Aggregate(
            Dictionary<string, string> source, JToken token)
        {
            source.Add(token.Path, token.ToString());

            return source;
        }
    }
}