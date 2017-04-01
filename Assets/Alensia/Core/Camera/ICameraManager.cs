using System;
using System.Collections.ObjectModel;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public interface ICameraManager
    {
        ICameraMode Mode { get; }

        ReadOnlyCollection<ICameraMode> AvailableModes { get; }

        event EventHandler<CameraChangeEventArgs> CameraChanged;

        T Switch<T>() where T : class, ICameraMode;

        IFirstPersonCamera ToFirstPerson(IActor target);

        IThirdPersonCamera ToThirdPerson(IActor target);

        void Follow(ITransformable target);
    }

    public class CameraChangeEventArgs : EventArgs
    {
        public readonly ICameraMode Mode;

        public CameraChangeEventArgs(ICameraMode mode)
        {
            Assert.IsNotNull(mode, "mode != null");

            Mode = mode;
        }
    }
}