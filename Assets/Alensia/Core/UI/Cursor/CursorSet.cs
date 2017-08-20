using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI.Cursor
{
    public class CursorSet : ScriptableObject, IDirectory<CursorDefinition>, IEditorSettings
    {
        protected IDictionary<string, CursorDefinition> CursorMap
        {
            get
            {
                lock (this)
                {
                    if (_cursorMap != null) return _cursorMap;

                    _cursorMap = new Dictionary<string, CursorDefinition>();

                    foreach (var cursor in _cursors.Concat<CursorDefinition>(_animatedCursors))
                    {
                        _cursorMap.Add(cursor.Name, cursor);
                    }

                    return _cursorMap;
                }
            }
        }

        [SerializeField] private StaticCursor[] _cursors;

        [SerializeField] private AnimatedCursor[] _animatedCursors;

        private IDictionary<string, CursorDefinition> _cursorMap;

        public bool Contains(string key) => CursorMap.ContainsKey(key);

        public CursorDefinition this[string key] => CursorMap[key];

        private void OnEnable()
        {
            _cursors = _cursors?.OrderBy(c => c.Name).ToArray();
            _animatedCursors = _animatedCursors?.OrderBy(c => c.Name).ToArray();
        }

        private void OnValidate() => _cursorMap = null;

        private void OnDestroy() => _cursorMap = null;
    }
}