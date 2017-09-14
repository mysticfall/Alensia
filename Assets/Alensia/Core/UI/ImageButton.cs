using Alensia.Core.UI.Property;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public class ImageButton : Button
    {
        protected override TextStyleSet DefaultTextStyleSet =>
            Style?.TextStyleSets?["ImageButton.Text"]?.Merge(base.DefaultTextStyleSet) ??
            base.DefaultTextStyleSet;

        protected override ImageAndColorSet DefaultBackgroundSet =>
            Style?.ImageAndColorSets?["ImageButton.Background"]?.Merge(base.DefaultBackgroundSet) ??
            base.DefaultBackgroundSet;

        protected override ImageAndColorSet DefaultIconSet =>
            Style?.ImageAndColorSets?["ImageButton.Icon"]?.Merge(base.DefaultIconSet) ??
            base.DefaultIconSet;

        protected override UIComponent CreatePristineInstance() => CreateInstance();

        public new static ImageButton CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/ImageButton");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<ImageButton>();
        }
    }
}