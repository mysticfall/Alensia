using Alensia.Core.Input;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public abstract class LocomotionControl<T> : Control.Control, ILocomotionControl<T>
        where T : class, ILocomotion
    {
        public T Locomotion { get; }

        public override bool Valid => base.Valid && Locomotion.Active.Value;

        protected LocomotionControl(T locomotion, IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(locomotion, "locomotion != null");

            Locomotion = locomotion;
        }
    }
}