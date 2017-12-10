using System;
using System.Globalization;
using Malee;
using UnityEngine;
using UnityEngine.Assertions;

// ReSharper disable NonReadonlyMemberInGetHashCode
namespace Alensia.Core.I18n
{
    [Serializable]
    public class LanguageTag
    {
        [SerializeField]
        private string _tag;

        public LanguageTag(string tag)
        {
            Assert.IsNotNull(tag, "tag != null");

            _tag = tag;
        }

        public CultureInfo ToCulture() => new CultureInfo(_tag);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && _tag == ((LanguageTag) obj)._tag;
        }

        public override int GetHashCode() => _tag != null ? _tag.GetHashCode() : 0;

        public override string ToString() => _tag;
    }

    [Serializable]
    public class LanguageTagList : ReorderableArray<LanguageTag>
    {
    }
}