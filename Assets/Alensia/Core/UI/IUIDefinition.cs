using System;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.UI
{
    public interface IUIDefinition : IEditorSettings, INamed, IComparable<IUIDefinition>
    {
        GameObject Item { get; }

        bool Enable { get; }

        int Order { get; }
    }
}