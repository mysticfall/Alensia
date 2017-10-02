using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character.Morph;
using Alensia.Core.Character.Morph.Generic;
using UnityEngine;

namespace Alensia.Demo.UMA
{
    public class ColorMorphPanel : MorphListPanel
    {
        public GameObject ColorItemPrefab;

        protected override void LoadMorphs(IReadOnlyList<IMorph> morphs)
        {
            base.LoadMorphs(morphs);

            var colors = morphs
                .OrderBy(m => m.Name)
                .Select(m => m as IMorph<Color>)
                .Where(m => m != null);

            foreach (var morph in colors)
            {
                var item = Context.Instantiate<ColorItem>(
                    ColorItemPrefab,
                    ContentPanel);

                item.Morph = morph;
            }
        }
    }
}