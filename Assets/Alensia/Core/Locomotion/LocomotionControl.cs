using Alensia.Core.Input;

namespace Alensia.Core.Locomotion
{
    public abstract class LocomotionControl<T> : Control.Control, ILocomotionControl<T>
        where T : class, ILocomotion
    {
        public const string Category = "Locomotion";

        public abstract T Locomotion { get; }

        public override bool Valid => base.Valid && Locomotion != null && Locomotion.Active;

        protected LocomotionControl(IInputManager inputManager) : base(inputManager)
        {
        }
    }
}