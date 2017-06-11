using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Alensia.Core.I18n
{
    public class DictionaryMessages : IMessages
    {
        public IMessages Parent { get; }

        private readonly IDictionary<string, string> _dictionary;

        public DictionaryMessages(
            IDictionary<string, string> dictionary, IMessages parent)
        {
            Assert.IsNotNull(dictionary, "dictionary != null");

            _dictionary = dictionary;

            Parent = parent;
        }

        public bool Contains(string key)
        {
            Assert.IsNotNull(key, "key != null");

            var found = _dictionary.ContainsKey(key);

            return found || Parent != null && Parent.Contains(key);
        }

        public string this[string key]
        {
            get
            {
                Assert.IsNotNull(key, "key != null");

                string value;

                _dictionary.TryGetValue(key, out value);

                return value ?? Parent?[key];
            }
        }
    }
}