using UnityEditor;
using UnityEngine;

namespace Alensia.Core.I18n
{
    [CustomPropertyDrawer(typeof(TranslatableText))]
    public class TranslatableTextPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var text = property.FindPropertyRelative("_text");
            var textKey = property.FindPropertyRelative("_textKey");

            EditorGUI.BeginProperty(position, label, text);

            var xMin = position.xMin;

            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginChangeCheck();

            var textValue = EditorGUI.TextField(position, label, text.stringValue);

            if (EditorGUI.EndChangeCheck())
            {
                text.stringValue = textValue;
            }

            EditorGUI.EndProperty();

            position.xMin = xMin;
            position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;


            EditorGUI.BeginProperty(position, label, textKey);

            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginChangeCheck();

            var textKeyValue = EditorGUI.TextField(
                position, $"{property.displayName} (I18n Key)", textKey.stringValue);

            if (EditorGUI.EndChangeCheck())
            {
                textKey.stringValue = textKeyValue;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            const int rows = 2;

            return base.GetPropertyHeight(property, label) * rows +
                   (rows - 1) * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}