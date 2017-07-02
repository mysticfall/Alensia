using UnityEditor;
using UnityEngine;

namespace Alensia.Core.UI
{
    public static class ComponentFactory
    {
        [MenuItem("GameObject/UI/Alensia/Button", false, 10)]
        public static Button CreateButton(MenuCommand command)
        {
            var button = Button.CreateInstance();

            GameObjectUtility.SetParentAndAlign(button.gameObject, command.context as GameObject);

            Undo.RegisterCreatedObjectUndo(button, "Create " + button.name);

            Selection.activeObject = button;

            return button;
        }

        [MenuItem("GameObject/UI/Alensia/Label", false, 10)]
        public static Label CreateLabel(MenuCommand command)
        {
            var label = Label.CreateInstance();

            GameObjectUtility.SetParentAndAlign(label.gameObject, command.context as GameObject);

            Undo.RegisterCreatedObjectUndo(label, "Create " + label.name);

            Selection.activeObject = label;

            return label;
        }

        [MenuItem("GameObject/UI/Alensia/Panel", false, 10)]
        public static Panel CreatePanel(MenuCommand command)
        {
            var panel = Panel.CreateInstance();

            GameObjectUtility.SetParentAndAlign(panel.gameObject, command.context as GameObject);

            Undo.RegisterCreatedObjectUndo(panel, "Create " + panel.name);

            Selection.activeObject = panel;

            return panel;
        }
    }
}