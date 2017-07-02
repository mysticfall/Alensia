using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI
{
    public abstract class UIDefinition : IEditorSettings, IComparable<UIDefinition>
    {
        public string Name;

        public GameObject Item;

        public bool Enable = true;

        public int Order = 0;

        public int CompareTo(UIDefinition obj) => Order.CompareTo(obj.Order);
    }
}