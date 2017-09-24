using System.Collections.Generic;
using UnityEngine;

namespace Alensia.Core.Common
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();
        }

        public static IEnumerable<Transform> GetChildren(this Component parent) => parent.transform.GetChildren();

        public static T FindComponent<T>(this Component parent, string path) where T : class =>
            parent.transform.Find(path)?.GetComponent<T>();

        public static T FindComponentInChildren<T>(this Component parent, string path) where T : class =>
            parent.transform.Find(path)?.GetComponentInChildren<T>();
    }
}