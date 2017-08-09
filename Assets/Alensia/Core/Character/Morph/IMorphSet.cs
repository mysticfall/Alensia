using System.Collections.Generic;
using UniRx;

namespace Alensia.Core.Character.Morph
{
    public interface IMorphSet
    {
        Race Race { get; set; }

        Sex Sex { get; set; }

        IReadOnlyList<IMorph> Morphs { get; }

        IObservable<Race> OnRaceChange { get; }

        IObservable<Sex> OnSexChange { get; }

        IObservable<IMorph> OnMorph { get; }

        IObservable<IEnumerable<IMorph>> OnMorphsChange { get; }
    }
}