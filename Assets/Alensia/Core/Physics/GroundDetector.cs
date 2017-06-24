using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Alensia.Core.Physics
{
    public abstract class GroundDetector : BaseObject, IGroundDetector
    {
        public abstract GroundDetectionSettings Settings { get; }

        public abstract Collider Target { get; }

        public IReadOnlyReactiveCollection<Collider> Grounds => _grounds;

        public IReadOnlyReactiveProperty<bool> Grounded =>
            Grounds.ObserveCountChanged().Select(c => c > 0).ToReadOnlyReactiveProperty();

        public IObservable<Unit> OnGroundHit => Grounded.Where(v => v).Select(_ => Unit.Default);

        public IObservable<Unit> OnGroundLeave => Grounded.Where(v => !v).Select(_ => Unit.Default);

        private readonly ReactiveCollection<Collider> _grounds;

        protected GroundDetector()
        {
            _grounds = new ReactiveCollection<Collider>();
        }

        protected virtual bool IsGround(Collider collider) => true;

        protected void OnDetectGround(IEnumerable<Collider> grounds)
        {
            lock (_grounds)
            {
                var collection = grounds.ToHashSet();

                var oldContacts = Grounds.Except(collection).ToHashSet();
                var contacts = collection.Except(Grounds).ToHashSet();

                oldContacts.ForEach(c => _grounds.Remove(c));
                contacts.ForEach(c => _grounds.Add(c));
            }
        }
    }
}