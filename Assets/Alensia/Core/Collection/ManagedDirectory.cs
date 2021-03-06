﻿using System.Collections.Generic;
using Alensia.Core.Common;

namespace Alensia.Core.Collection
{
    public abstract class ManagedDirectory<T> : ManagedMonoBehavior, IDirectory<T>
        where T : class, INamed
    {
        protected abstract IEnumerable<T> Items { get; }

        protected IDirectory<T> Directory
        {
            get
            {
                lock (this)
                {
                    if (_directory != null) return _directory;

                    _directory = new SimpleDirectory<T>(Items);

                    return _directory;
                }
            }
        }

        private IDirectory<T> _directory;

        public bool Contains(string key) => Directory.Contains(key);

        public T this[string key] => Directory[key];

        protected void ClearCache() => _directory = null;

        protected override void OnDisposed()
        {
            ClearCache();

            base.OnDisposed();
        }
    }
}