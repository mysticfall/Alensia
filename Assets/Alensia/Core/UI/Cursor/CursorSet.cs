using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI.Cursor
{
    public class CursorSet : PersistedDirectory<CursorDefinition>, INamed
    {
        public string Name => name;

        [SerializeField] private StaticCursor[] _cursors;

        [SerializeField] private AnimatedCursor[] _animatedCursors;

        private void OnEnable()
        {
            _cursors = _cursors?.OrderBy(c => c.Name).ToArray();
            _animatedCursors = _animatedCursors?.OrderBy(c => c.Name).ToArray();
        }

        protected override IEnumerable<CursorDefinition> Items => 
            _cursors.Concat<CursorDefinition>(_animatedCursors);
    }
}