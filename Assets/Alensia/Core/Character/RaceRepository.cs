using System.Collections.Generic;
using Alensia.Core.Collection;
using Malee;
using UnityEngine;

namespace Alensia.Core.Character
{
    public class RaceRepository : ManagedDirectory<Race>, IRaceRepository
    {
        public IEnumerable<Race> Races => _races;

        [SerializeField, Reorderable] private RaceList _races;

        protected override IEnumerable<Race> Items => _races;
    }
}