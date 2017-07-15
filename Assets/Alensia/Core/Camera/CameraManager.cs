using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Common;
using UniRx;
using UnityEngine.Assertions;

namespace Alensia.Core.Camera
{
    public class CameraManager : BaseObject, ICameraManager
    {
        public ICameraMode Mode
        {
            get { return _mode.Value; }
            set
            {
                Assert.IsNotNull(value, "value != null");

                _mode.Value = value;
            }
        }

        public IReadOnlyCollection<ICameraMode> AvailableModes { get; }

        public IObservable<ICameraMode> OnCameraModeChange => _mode;

        private readonly IReactiveProperty<ICameraMode> _mode;

        public CameraManager(List<ICameraMode> modes)
        {
            Assert.IsNotNull(modes, "modes != null");
            Assert.IsTrue(modes.Any(), "modes.Any()");

            AvailableModes = modes.AsReadOnly();

            _mode = new ReactiveProperty<ICameraMode>();

            _mode.Pairwise().Subscribe(Switch).AddTo(this);
        }

        private static void Switch(Pair<ICameraMode> cameras)
        {
            cameras.Previous?.Deactivate();
            cameras.Current?.Activate();
        }

        public T Switch<T>() where T : class, ICameraMode
        {
            var camera = AvailableModes.FirstOrDefault(m => m is T) as T;

            if (camera != null)
            {
                Mode = camera;
            }

            return camera;
        }

        public IFirstPersonCamera ToFirstPerson(ICharacter target)
        {
            var camera = Switch<IFirstPersonCamera>();

            if (camera == null) return null;

            camera.Initialize(target);

            Mode = camera;

            return camera;
        }

        public IThirdPersonCamera ToThirdPerson(ICharacter target)
        {
            var camera = Switch<IThirdPersonCamera>();

            if (camera == null) return null;

            camera.Initialize(target);

            Mode = camera;

            return camera;
        }
    }
}