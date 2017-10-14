using System;
using System.Collections.Generic;

namespace Alensia.Core.Character.Morph
{
    public interface IMorphSet
    {
        Race Race { get; set; }

        Sex Sex { get; set; }

        IEnumerable<IMorph> Morphs { get; }

        IObservable<Race> OnRaceChange { get; }

        IObservable<Sex> OnSexChange { get; }

        IObservable<IMorph> OnMorph { get; }

        IObservable<IEnumerable<IMorph>> OnMorphsChange { get; }
    }
}