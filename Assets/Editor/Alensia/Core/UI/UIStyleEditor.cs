using UnityEditor;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace Alensia.Core.UI
{
    [CustomEditor(typeof(UIStyle))]
    public class UIStyleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BeginChangeCheck();

            ObjectField(serializedObject.FindProperty("_parent"));

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            BeginChangeCheck();

            ObjectField(serializedObject.FindProperty("_cursorSet"));

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            BeginChangeCheck();

            PropertyField(serializedObject.FindProperty("_imagesAndColors"), true);

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            BeginChangeCheck();

            PropertyField(serializedObject.FindProperty("_imageAndColorSets"), true);

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            BeginChangeCheck();

            PropertyField(serializedObject.FindProperty("_textStyles"), true);

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            BeginChangeCheck();

            PropertyField(serializedObject.FindProperty("_textStyleSets"), true);

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}