using System;
using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Character;
using Alensia.Core.Common;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Camera
{
    public class CameraManager : ManagedMonoBehavior, ICameraManager, ILateTickable
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

        public IEnumerable<ICameraMode> AvailableModes => _availableModes?.ToList();

        public IObservable<ICameraMode> OnCameraModeChange => _mode;

        public IObservable<Unit> OnCameraUpdate => _cameraUpdate;

        [Inject] private IList<ICameraMode> _availableModes;

        private readonly IReactiveProperty<ICameraMode> _mode;

        private readonly ISubject<Unit> _cameraUpdate;

        public CameraManager()
        {
            _mode = new ReactiveProperty<ICameraMode>();
            _cameraUpdate = new Subject<Unit>();
        }

        protected override void OnInitialized()
        {
            _mode
                .Pairwise()
                .Subscribe(Switch, Debug.LogError)
                .AddTo(this);

            base.OnInitialized();
        }

        protected override void OnDisposed()
        {
            _cameraUpdate.OnCompleted();

            base.OnDisposed();
        }

        private static void Switch(Pair<ICameraMode> cameras)
        {
            cameras.Previous?.Deactivate();
            cameras.Current?.Activate();
        }

        public T Switch<T>() where T : class, ICameraMode
        {
            var cam = AvailableModes.FirstOrDefault(m => m is T) as T;

            if (cam != null)
            {
                Mode = cam;
            }

            return cam;
        }

        public IFirstPersonCamera ToFirstPerson(ICharacter target)
        {
            var cam = Switch<IFirstPersonCamera>();

            if (cam == null) return null;

            cam.Track(target);

            Mode = cam;

            return cam;
        }

        public IThirdPersonCamera ToThirdPerson(ICharacter target)
        {
            var cam = Switch<IThirdPersonCamera>();

            if (cam == null) return null;

            cam.Track(target);

            Mode = cam;

            return cam;
        }

        public void LateTick()
        {
            var updatable = Mode as IUpdatableCamera;

            if (updatable == null) return;

            updatable.UpdatePosition();

            _cameraUpdate.OnNext(Unit.Default);
        }
    }
}