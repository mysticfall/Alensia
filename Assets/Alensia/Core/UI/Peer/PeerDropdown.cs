using UnityEngine;

namespace Alensia.Core.UI.Peer
{
    public class PeerDropdown : UnityEngine.UI.Dropdown, IUIContextHolder
    {
        public Dropdown Parent => GetComponentInParent<Dropdown>();

        public IUIContext Context => Parent.Context;

        protected override GameObject CreateDropdownList(GameObject listTemplate)
        {
            var list = base.CreateDropdownList(listTemplate);

            var panel = list.GetComponent<ScrollPanel>();

            panel.Initialize(Context);

            return list;
        }

        protected override DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            var item = base.CreateItem(itemTemplate);
            var toggle = item.GetComponent<ToggleButton>();

            toggle.Initialize(Context);

            return item;
        }
    }
}