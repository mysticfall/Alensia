using UnityEngine;
using Zenject;

namespace Alensia.Core.Character
{
    public class SimpleHumanoid : Humanoid
    {
        public override IRace Race => RaceRepository[_race];

        public override Sex Sex => _sex;

        [Inject]
        protected IRaceRepository RaceRepository { get; }

        [SerializeField] private string _race;

        [SerializeField] private Sex _sex;
    }
}