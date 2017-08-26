using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Alensia.Core.I18n
{
    [CustomPropertyDrawer(typeof(TranslatableText))]
    public class TranslatableTextPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var text = property.FindPropertyRelative("_text");
            var textKey = property.FindPropertyRelative("_textKey");

            BeginProperty(position, label, text);

            var xMin = position.xMin;

            position.height = EditorGUIUtility.singleLineHeight;

            BeginChangeCheck();

            var textValue = TextField(position, label, text.stringValue);

            if (EndChangeCheck())
            {
                text.stringValue = textValue;
            }

            EndProperty();

            position.xMin = xMin;
            position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;


            BeginProperty(position, label, textKey);

            position.height = EditorGUIUtility.singleLineHeight;

            BeginChangeCheck();

            var textKeyValue = TextField(
                position, $"{property.displayName} (I18n Key)", textKey.stringValue);

            if (EndChangeCheck())
            {
                textKey.stringValue = textKeyValue;
            }

            EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            const int rows = 2;

            return base.GetPropertyHeight(property, label) * rows +
                   (rows - 1) * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}