using Alensia.Core.Animation;
using Alensia.Core.Common;
using Alensia.Core.Locomotion;
using Alensia.Core.Locomotion.Generic;
using Alensia.Core.Sensor;
using Alensia.Core.Sensor.Generic;

namespace Alensia.Core.Character
{
    public interface ICharacter : IEntity, IAnimatable, ISeeing, ILocomotive
    {
    }

    namespace Generic
    {
        public interface ICharacter<out TVision, out TLocomotion> : ICharacter,
            ISeeing<TVision>, ILocomotive<TLocomotion>
            where TVision : IVision
            where TLocomotion : ILocomotion
        {
        }
    }
}