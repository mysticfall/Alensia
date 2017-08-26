using UnityEditor;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUILayout;

namespace Alensia.Core.UI
{
    [CustomEditor(typeof(EditorUIContext))]
    public class EditorUIContextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BeginChangeCheck();

            PropertyField(serializedObject.FindProperty("_locale"));

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            BeginChangeCheck();

            ObjectField(serializedObject.FindProperty("_style"));

            if (EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}