using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UniRx;

namespace Alensia.Core.Character.Morph
{
    public abstract class MorphSet : BaseObject, IMorphSet
    {
        public Race Race
        {
            get { return _race.Value; }
            set
            {
                if (Equals(_race.Value, value)) return;

                if (Initialized) ChangeRace(value);

                _race.Value = value;
            }
        }

        public Sex Sex
        {
            get { return _sex.Value; }
            set
            {
                if (_sex.Value == value) return;

                if (Initialized) ChangeSex(value);

                _sex.Value = value;
            }
        }

        public IReadOnlyList<IMorph> Morphs { get; private set; }

        public IObservable<Race> OnRaceChange => _race;

        public IObservable<Sex> OnSexChange => _sex;

        public IObservable<IMorph> OnMorph => _morph;

        private readonly IReactiveProperty<Sex> _sex;

        private readonly IReactiveProperty<Race> _race;

        private readonly ISubject<IMorph> _morph;

        private readonly CompositeDisposable _morphListeners;

        protected MorphSet()
        {
            _sex = new ReactiveProperty<Sex>();
            _race = new ReactiveProperty<Race>();
            _morph = new Subject<IMorph>();
            _morphListeners = new CompositeDisposable();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            OnSexChange.AsUnitObservable()
                .Merge(OnRaceChange.AsUnitObservable())
                .Subscribe(_ => UpdateMorphs())
                .AddTo(this);
        }

        private void UpdateMorphs()
        {
            lock (this)
            {
                _morphListeners.Clear();

                Morphs = CreateMorphs().ToList().AsReadOnly();

                foreach (var morph in Morphs)
                {
                    morph.OnChange.Subscribe(_ =>
                    {
                        ApplyMorph(morph);
                        _morph.OnNext(morph);
                    }).AddTo(_morphListeners);
                }
            }
        }

        protected override void OnDisposed()
        {
            _morph.OnCompleted();
            _morphListeners.Dispose();

            base.OnDisposed();
        }

        protected abstract IEnumerable<IMorph> CreateMorphs();

        protected abstract void ChangeSex(Sex sex);

        protected abstract void ChangeRace(Race race);

        protected abstract void ApplyMorph(IMorph morph);
    }
}