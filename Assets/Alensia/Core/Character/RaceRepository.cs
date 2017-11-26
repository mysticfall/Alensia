using System.Collections.Generic;
using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Character
{
    public class RaceRepository : ManagedDirectory<Race>, IRaceRepository
    {
        public IEnumerable<Race> Races => _races;

        [SerializeField] private Race[] _races;

        protected override IEnumerable<Race> Items => _races;
    }
}