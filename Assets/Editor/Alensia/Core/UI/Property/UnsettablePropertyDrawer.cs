using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Alensia.Core.UI.Property
{
    [CustomPropertyDrawer(typeof(UnsettableProperty<>))]
    public class UnsettablePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative("_hasValue");
            var valueProperty = property.FindPropertyRelative("_value");

            BeginProperty(position, label, property);

            var xMin = position.xMin;
            var height = position.height;

            position.height = EditorGUIUtility.singleLineHeight;

            LabelField(position, label);

            position.xMin = xMin + EditorGUIUtility.labelWidth - indentLevel * 15f;

            BeginChangeCheck();

            var useDefault = ToggleLeft(position, "Use Default", !hasValueProperty.boolValue);

            if (EndChangeCheck())
            {
                hasValueProperty.boolValue = !useDefault;
            }

            if (!useDefault)
            {
                position.height = height;

                position.xMin = xMin;
                position.yMin += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                PropertyField(position, valueProperty, new GUIContent(" "));
            }

            EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var hasValueProperty = property.FindPropertyRelative("_hasValue");

            var height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var rows = hasValueProperty.boolValue ? 2 : 1;

            return height * rows;
        }
    }

    [CustomPropertyDrawer(typeof(UnsettableInt))]
    public class UnsettableIntPropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableFloat))]
    public class UnsettableFloatPropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableBool))]
    public class UnsettableBoolPropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableColor))]
    public class UnsettableColorPropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableSprite))]
    public class UnsettableSpritePropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableImageType))]
    public class UnsettableImageTypePropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableFont))]
    public class UnsettableFontPropertyDrawer : UnsettablePropertyDrawer
    {
    }
    
    [CustomPropertyDrawer(typeof(UnsettableFontStyle))]
    public class UnsettableFontStylePropertyDrawer : UnsettablePropertyDrawer
    {
    }
    
    [CustomPropertyDrawer(typeof(UnsettableTextAnchor))]
    public class UnsettableTextAnchorPropertyDrawer : UnsettablePropertyDrawer
    {
    }

    [CustomPropertyDrawer(typeof(UnsettableHorizontalWrapMode))]
    public class UnsettableHorizontalWrapModePropertyDrawer : UnsettablePropertyDrawer
    {
    }
    
    [CustomPropertyDrawer(typeof(UnsettableVerticalWrapMode))]
    public class UnsettableVerticalWrapModePropertyDrawer : UnsettablePropertyDrawer
    {
    }
}