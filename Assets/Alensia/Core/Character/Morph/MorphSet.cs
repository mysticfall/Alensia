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

        public IReadOnlyList<IMorph> Morphs => _morphs.Value;

        public IObservable<Race> OnRaceChange => _race;

        public IObservable<Sex> OnSexChange => _sex;

        public IObservable<IMorph> OnMorph => _morphChange;

        public IObservable<IEnumerable<IMorph>> OnMorphsChange =>
            _morphs.Cast<IReadOnlyList<IMorph>, IEnumerable<IMorph>>();

        private readonly IReactiveProperty<Sex> _sex;

        private readonly IReactiveProperty<Race> _race;

        private readonly IReactiveProperty<IReadOnlyList<IMorph>> _morphs;

        private readonly ISubject<IMorph> _morphChange;

        private readonly CompositeDisposable _morphListeners;

        protected MorphSet()
        {
            _sex = new ReactiveProperty<Sex>();
            _race = new ReactiveProperty<Race>();
            _morphs = new ReactiveProperty<IReadOnlyList<IMorph>>(
                Enumerable.Empty<IMorph>().ToList());

            _morphChange = new Subject<IMorph>();
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

                _morphs.Value = CreateMorphs().ToList().AsReadOnly();

                foreach (var morph in Morphs)
                {
                    morph.OnChange.Subscribe(_ =>
                    {
                        ApplyMorph(morph);
                        _morphChange.OnNext(morph);
                    }).AddTo(_morphListeners);
                }
            }
        }

        protected override void OnDisposed()
        {
            _morphChange.OnCompleted();
            _morphListeners.Dispose();

            base.OnDisposed();
        }

        protected abstract IEnumerable<IMorph> CreateMorphs();

        protected abstract void ChangeSex(Sex sex);

        protected abstract void ChangeRace(Race race);

        protected abstract void ApplyMorph(IMorph morph);
    }
}