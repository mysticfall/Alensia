using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static System.String;
using static UnityEditor.EditorGUI;

namespace Alensia.Core.I18n
{
    [CustomPropertyDrawer(typeof(LanguageTag))]
    public class LanguageTagPropertyDrawer : PropertyDrawer
    {
        public CultureInfo[] SupportedLocales { get; }

        public LanguageTagPropertyDrawer()
        {
            SupportedLocales = CultureInfo.GetCultures(CultureTypes.AllCultures);

            var comparator = Comparer<CultureInfo>.Create(
                (c1, c2) => Compare(c1.DisplayName, c2.DisplayName, StringComparison.Ordinal));

            Array.Sort(SupportedLocales, comparator);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BeginProperty(position, label, property);

            var tag = property.FindPropertyRelative("_tag");
            var value = tag.stringValue;

            var selected = value == null
                ? 0
                : SupportedLocales
                    .Select((l, i) => new {i, l})
                    .Where(t => t.l.Name == value)
                    .Select(t => t.i)
                    .FirstOrDefault();

            var options = SupportedLocales.Select(c => c.DisplayName).ToArray();

            selected = Popup(position, label.text, selected, options);

            tag.stringValue = SupportedLocales[selected].Name;

            EndProperty();
        }
    }
}