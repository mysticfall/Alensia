using Alensia.Core.Animation;
using Alensia.Core.Common;
using Alensia.Core.Locomotion;
using Alensia.Core.Locomotion.Generic;

namespace Alensia.Core.Character
{
    public interface ICharacter : IEntity, IAnimatable, ILocomotive
    {
    }

    namespace Generic
    {
        public interface ICharacter<out TLocomotion> : ICharacter, ILocomotive<TLocomotion>
            where TLocomotion : ILocomotion
        {
        }
    }
}