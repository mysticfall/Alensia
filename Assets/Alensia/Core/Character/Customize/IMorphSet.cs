using System;
using System.Collections.Generic;

namespace Alensia.Core.Character.Customize
{
    public interface IMorphSet
    {
        IRace Race { get; set; }

        Sex Sex { get; set; }

        IEnumerable<IMorph> Morphs { get; }

        IObservable<IRace> OnRaceChange { get; }

        IObservable<Sex> OnSexChange { get; }

        IObservable<IMorph> OnMorph { get; }

        IObservable<IEnumerable<IMorph>> OnMorphsChange { get; }
    }
}