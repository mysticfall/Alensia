using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alensia.Core.UI.Screen
{
    public interface IScreen : IUIElement
    {
        Canvas Canvas { get; }

        IEnumerable<IComponentHandler> Items { get; }

        T FindUI<T>() where T : class, IComponentHandler;

        T FindUI<T>(string name) where T : class, IComponentHandler;

        /// <exception cref="ArgumentException">
        /// If there is no UI definition with the given name.
        /// </exception>
        T ShowUI<T>(string name) where T : class, IComponentHandler;
    }
}