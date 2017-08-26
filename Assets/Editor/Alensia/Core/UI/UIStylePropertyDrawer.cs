using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace Alensia.Core.UI
{
    [CustomPropertyDrawer(typeof(UIStyle))]
    public class UIStylePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BeginChangeCheck();

            var style = ObjectField(position, label, property.objectReferenceValue, typeof(UIStyle), false);

            if (EndChangeCheck())
            {
                property.objectReferenceValue = style;
            }
        }
    }
}