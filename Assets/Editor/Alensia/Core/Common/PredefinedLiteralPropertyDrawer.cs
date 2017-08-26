using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UniRx;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Alensia.Core.Common
{
    [CustomPropertyDrawer(typeof(PredefinedLiteralAttribute))]
    public class PredefinedLiteralPropertyDrawer : InspectorDisplayDrawer
    {
        public IList<string> Values
        {
            get
            {
                var attr = (PredefinedLiteralAttribute) attribute;

                var fields = attr.Type.GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy);

                return fields
                    .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                    .Select(f => (string) f.GetValue(null)).ToList();
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.type == "string")
            {
                EmitPropertyField(position, property, label);
            }
            else
            {
                base.OnGUI(position, property, label);
            }
        }

        protected override void EmitPropertyField(Rect position, SerializedProperty property, GUIContent label)
        {
            var values = Values;

            var options = new List<string>(values.Count + 2);
            var offset = 0;

            var attr = (PredefinedLiteralAttribute) attribute;

            var customIndex = -1;

            if (attr.Optional)
            {
                options.Add("(None)");
                offset++;
            }

            if (attr.AllowCustom)
            {
                options.Add("(Custom)");

                customIndex = offset;
                offset++;
            }

            options.AddRange(values);

            BeginProperty(position, label, property);

            var index = 0;
            var value = property.stringValue;

            if (!string.IsNullOrEmpty(value))
            {
                var i = values.IndexOf(value);

                if (i > -1)
                {
                    index = i + offset;
                }
                else if (attr.AllowCustom)
                {
                    index = customIndex;
                }
            }

            var height = position.height;

            position.height = EditorGUIUtility.singleLineHeight;

            BeginChangeCheck();

            var selected = Popup(position, label.text, index, options.ToArray());

            if (EndChangeCheck())
            {
                if (offset > selected)
                {
                    if (attr.Optional && selected == 0)
                    {
                        property.stringValue = null;
                    }
                    else if (attr.AllowCustom)
                    {
                        property.stringValue = "(Please enter value here)";
                    }
                }
                else
                {
                    property.stringValue = values[selected - offset];
                }
            }

            position.height = height;

            if (selected == customIndex)
            {
                position.xMin += EditorGUIUtility.labelWidth - indentLevel * 15f;
                position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                TextField(position, property.stringValue);
            }

            EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 1;

            var attr = (PredefinedLiteralAttribute) attribute;

            if (attr.AllowCustom)
            {
                SerializedProperty valueProperty;

                if (property.type == "string")
                {
                    valueProperty = property;
                }
                else
                {
                    var fieldName = (attribute as InspectorDisplayAttribute)?.FieldName ?? "value";

                    valueProperty = property.FindPropertyRelative(fieldName);
                }

                var value = valueProperty.stringValue;

                if (string.IsNullOrEmpty(value))
                {
                    lines = attr.Optional ? 1 : 2;
                }
                else
                {
                    lines = Values.Contains(value) ? 1 : 2;
                }
            }

            return base.GetPropertyHeight(property, label) * lines;
        }
    }
}