using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;

namespace Alensia.Core.Collection
{
    public class SimpleDirectory<T> : Directory<T> where T : class, INamed
    {
        public SimpleDirectory() : this(Enumerable.Empty<T>())
        {
        }

        public SimpleDirectory(IEnumerable<T> items)
        {
            Items = items ?? Enumerable.Empty<T>();
        }

        protected override IEnumerable<T> Items { get; }
    }
}