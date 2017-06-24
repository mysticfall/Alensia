using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Actor;
using Alensia.Core.Common;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public class CameraManager : BaseObject, ICameraManager
    {
        public IReadOnlyReactiveProperty<ICameraMode> Mode { get; }

        public IReadOnlyCollection<ICameraMode> AvailableModes { get; }

        private readonly IReactiveProperty<ICameraMode> _mode;

        public CameraManager(List<ICameraMode> modes)
        {
            Assert.IsNotNull(modes, "modes != null");
            Assert.IsTrue(modes.Any(), "modes.Any()");

            AvailableModes = modes.AsReadOnly();

            _mode = new ReactiveProperty<ICameraMode>();
            Mode = new ReadOnlyReactiveProperty<ICameraMode>(_mode);

            Mode.Pairwise().Subscribe(Switch).AddTo(this);
        }

        private static void Switch(Pair<ICameraMode> cameras)
        {
            cameras.Previous?.Deactivate();
            cameras.Current?.Activate();
        }

        public T Switch<T>() where T : class, ICameraMode
        {
            return AvailableModes.FirstOrDefault(m => m is T) as T;
        }

        public IFirstPersonCamera ToFirstPerson(IActor target)
        {
            var camera = Switch<IFirstPersonCamera>();

            if (camera == null) return null;

            camera.Initialize(target);

            _mode.Value = camera;

            return camera;
        }

        public IThirdPersonCamera ToThirdPerson(IActor target)
        {
            var camera = Switch<IThirdPersonCamera>();

            if (camera == null) return null;

            camera.Initialize(target);

            _mode.Value = camera;

            return camera;
        }
    }
}