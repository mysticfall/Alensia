using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public class DictionaryTranslationSet : ITranslationSet
    {
        private readonly IDictionary<string, string> _dictionary;

        public DictionaryTranslationSet(IDictionary<string, string> dictionary)
        {
            Assert.IsNotNull(dictionary, "dictionary != null");

            _dictionary = dictionary;
        }

        public bool Contains(string key) => _dictionary.ContainsKey(key);

        public string this[string key]
        {
            get
            {
                string value;

                _dictionary.TryGetValue(key, out value);

                return value;
            }
        }
    }
}