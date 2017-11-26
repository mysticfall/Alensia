using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Character
{
    public class RaceRepository : ManagedDirectory<Race>, IRaceRepository
    {
        public IEnumerable<Race> Races => _races;

        [SerializeField] private Race[] _races;

        public RaceRepository() : this(new List<Race> {Race.Human})
        {
        }

        public RaceRepository(ICollection<Race> races)
        {
            Assert.IsNotNull(races, "races != null");

            _races = races.ToArray();
        }

        protected override IEnumerable<Race> Items => _races;
    }
}