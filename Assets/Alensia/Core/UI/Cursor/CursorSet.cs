using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using Malee;
using UnityEngine;

namespace Alensia.Core.UI.Cursor
{
    public class CursorSet : PersistedDirectory<CursorDefinition>, INamed
    {
        public string Name => name;

        [SerializeField, Reorderable] private StaticCursorList _cursors;

        [SerializeField, Reorderable] private AnimatedCursorList _animatedCursors;

        protected override IEnumerable<CursorDefinition> Items => 
            _cursors.Concat<CursorDefinition>(_animatedCursors);

        [Serializable]
        internal class StaticCursorList : ReorderableArray<StaticCursor>
        {
        }

        [Serializable]
        internal class AnimatedCursorList : ReorderableArray<AnimatedCursor>
        {
        }
    }
}