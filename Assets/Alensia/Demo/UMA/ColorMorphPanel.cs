using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character.Customize;
using Alensia.Core.Character.Customize.Generic;
using Alensia.Core.UI;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public class ColorMorphPanel : MorphListPanel
    {
        public GameObject ColorItemPrefab;

        protected override void LoadMorphs(IReadOnlyList<IMorph> morphs)
        {
            base.LoadMorphs(morphs);

            var runtimeContext = Context as IRuntimeUIContext;

            var colors = morphs
                .OrderBy(m => m.Name)
                .Select(m => m as IMorph<Color>)
                .Where(m => m != null);

            if (runtimeContext == null) return;

            foreach (var morph in colors)
            {
                var item = runtimeContext.Instantiate<ColorItem>(ColorItemPrefab, ContentPanel);

                item.Morph = morph;
            }
        }
    }
}