using System;
using UnityEditor;
using UnityEngine;

namespace Alensia.Core.UI
{
    public static class ComponentFactory
    {
        [MenuItem("GameObject/UI/Alensia/Button", false, 10)]
        public static Button CreateButton(MenuCommand command) => CreateComponent(command, Button.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Dropdown", false, 10)]
        public static Dropdown CreateDropdown(MenuCommand command) => CreateComponent(command, Dropdown.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Label", false, 10)]
        public static Label CreateLabel(MenuCommand command) => CreateComponent(command, Label.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Panel", false, 10)]
        public static Panel CreatePanel(MenuCommand command) => CreateComponent(command, Panel.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Slider", false, 10)]
        public static Slider CreateSlider(MenuCommand command) => CreateComponent(command, Slider.CreateInstance);

        [MenuItem("GameObject/UI/Alensia/Scroll Panel", false, 10)]
        public static ScrollPanel CreateScrollPanel(MenuCommand command) => CreateComponent(command, ScrollPanel.CreateInstance);

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