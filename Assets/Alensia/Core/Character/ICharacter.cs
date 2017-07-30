using Alensia.Core.Animation;
using Alensia.Core.Common;
using Alensia.Core.Locomotion;

namespace Alensia.Core.Character
{
    public interface ICharacter : IEntity, IAnimatable
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