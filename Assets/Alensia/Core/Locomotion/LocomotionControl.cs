using Alensia.Core.Input;
using UnityEngine.Assertions;

namespace Alensia.Core.Locomotion
{
    public abstract class LocomotionControl<T> : Control.Control, ILocomotionControl<T>
        where T : class, ILocomotion
    {
        public T Locomotion { get; private set; }

        public override bool Valid
        {
            get { return base.Valid && Locomotion.Active; }
        }

        protected LocomotionControl(T locomotion, IInputManager inputManager) : base(inputManager)
        {
            Assert.IsNotNull(locomotion, "locomotion != null");

            Locomotion = locomotion;
        }
    }
}