using System;
using UnityEditor;
using UnityEngine;

namespace Alensia.Core.UI
{
    public static class ComponentFactory
    {
        [MenuItem("GameObject/UI/Alensia/Button", false, 10)]
        public static Button CreateButton(MenuCommand command) => CreateComponent(command, Button.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Label", false, 10)]
        public static Label CreateLabel(MenuCommand command) => CreateComponent(command, Label.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Panel", false, 10)]
        public static Panel CreatePanel(MenuCommand command) => CreateComponent(command, Panel.CreateInstance);

        private static T CreateComponent<T>(
            MenuCommand command, Func<T> factory) where T : UIComponent
        {
            var component = factory.Invoke();

            GameObjectUtility.SetParentAndAlign(
                component.gameObject, command.context as GameObject);

            Undo.RegisterCreatedObjectUndo(component, "Create " + component.name);

            Selection.activeObject = component;

            return component;
        }
    }
}