using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI.Cursor
{
    public class CursorSet : ScriptableObject, ICursorSet, IEditorSettings
    {
        protected IDictionary<string, ICursorDefinition> CursorMap
        {
            get
            {
                lock (this)
                {
                    if (_cursorMap != null) return _cursorMap;

                    _cursorMap = new Dictionary<string, ICursorDefinition>();

                    foreach (var cursor in _cursors.Concat<ICursorDefinition>(_animatedCursors))
                    {
                        _cursorMap.Add(cursor.Name, cursor);
                    }

                    return _cursorMap;
                }
            }
        }

        [SerializeField] private StaticCursor[] _cursors;

        [SerializeField] private AnimatedCursor[] _animatedCursors;

        private IDictionary<string, ICursorDefinition> _cursorMap;

        public bool Contains(string key) => CursorMap.ContainsKey(key);

        public ICursorDefinition this[string key] => CursorMap[key];

        private void OnEnable()
        {
            _cursors = _cursors?.OrderBy(c => c.Name).ToArray();
            _animatedCursors = _animatedCursors?.OrderBy(c => c.Name).ToArray();
        }

        private void OnValidate() => _cursorMap = null;

        private void OnDestroy() => _cursorMap = null;
    }
}