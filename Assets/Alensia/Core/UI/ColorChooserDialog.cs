using Optional;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.UI
{
    public class ColorChooserDialog : Dialog<Color>
    {
        public ColorChooser Chooser => _chooser ?? (_chooser = ContentPanel.GetComponentInChildren<ColorChooser>());

        protected override Option<Color> Value => Option.Some(Chooser.Value);

        [SerializeField, HideInInspector] private ColorChooser _chooser;

        public new static ColorChooserDialog CreateInstance()
        {
            var prefab = Resources.Load<GameObject>("UI/Components/ColorChooserDialog");

            Assert.IsNotNull(prefab, "prefab != null");

            return Instantiate(prefab).GetComponent<ColorChooserDialog>();
        }
    }
}