using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI.Cursor
{
    public class CursorSet : ScriptableObject, ICursorSet, IEditorSettings
    {
        [SerializeField]
        private StaticCursor[] _cursors;

        [SerializeField]
        private AnimatedCursor[] _animatedCursors;

        private Dictionary<string, ICursorDefinition> _cursorMap;

        protected virtual void OnEnable()
        {
            _cursors = _cursors?.OrderBy(c => c.Name).ToArray();
            _animatedCursors = _animatedCursors?.OrderBy(c => c.Name).ToArray();
        }

        protected virtual void OnDestroy() => _cursorMap = null;

        public bool Contains(string key) => _cursorMap != null && _cursorMap.ContainsKey(key);

        public ICursorDefinition this[string key]
        {
            get
            {
                lock (this)
                {
                    if (_cursorMap != null) return _cursorMap[key];

                    _cursorMap = new Dictionary<string, ICursorDefinition>();

                    foreach (var cursor in _cursors.Concat<ICursorDefinition>(_animatedCursors))
                    {
                        _cursorMap.Add(cursor.Name, cursor);
                    }

                    return _cursorMap[key];
                }
            }
        }
    }
}