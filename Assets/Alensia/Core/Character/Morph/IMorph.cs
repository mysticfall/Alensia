using System;
using Alensia.Core.Common;

namespace Alensia.Core.Character.Morph
{
    public interface IMorph : INamed
    {
        object Value { get; set; }

        object DefaultValue { get; }

        IObservable<object> OnChange { get; }
    }

    namespace Generic
    {
        public interface IMorph<T> : IMorph
        {
            new T Value { get; set; }

            new T DefaultValue { get; }

            new IObservable<T> OnChange { get; }
        }
    }
}