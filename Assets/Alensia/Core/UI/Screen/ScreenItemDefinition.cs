using System;
using Malee;
using UnityEngine;
using UnityEngine.Assertions;

// ReSharper disable MemberInitializerValueIgnored
namespace Alensia.Core.UI.Screen
{
    [Serializable]
    public class ScreenItemDefinition : IUIDefinition
    {
        public string Name => _name;

        public GameObject Item => _item;

        public bool Singleton => _singleton;

        public bool Enable => _enable;

        public int Order => _order;

        public ScreenItemDefinition(
            string name, GameObject item, bool singleton = false, bool enable = true, int order = 0)
        {
            Assert.IsNotNull(name, "name != null");
            Assert.IsNotNull(item, "item != null");

            _name = name;
            _item = item;
            _singleton = singleton;
            _enable = enable;
            _order = order;
        }

        [SerializeField] private string _name;

        [SerializeField] private GameObject _item;

        [SerializeField] private bool _singleton;

        [SerializeField] private bool _enable = true;

        [SerializeField] private int _order;

        public int CompareTo(IUIDefinition obj) => Order.CompareTo(obj.Order);
    }

    [Serializable]
    internal class ScreenItemDefinitionList : ReorderableArray<ScreenItemDefinition>
    {
    }
}