using System;
using System.Collections.Generic;

namespace Alensia.Core.Character.Morph
{
    public interface IMorphSet
    {
        IMorphableRace Race { get; set; }

        Sex Sex { get; set; }

        IEnumerable<IMorph> Morphs { get; }

        IObservable<IRace> OnRaceChange { get; }

        IObservable<Sex> OnSexChange { get; }

        IObservable<IMorph> OnMorph { get; }

        IObservable<IEnumerable<IMorph>> OnMorphsChange { get; }
    }
}