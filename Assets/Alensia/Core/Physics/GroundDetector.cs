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

        public ISet<Collider> GroundContacts => _grounds.ToHashSet();

        public bool Grounded => _grounds.Any();

        public IObservable<Unit> OnGroundHit => OnGroundedStateChange.Where(v => v).AsUnitObservable();

        public IObservable<Unit> OnGroundLeave => OnGroundedStateChange.Where(v => !v).AsUnitObservable();

        public IObservable<bool> OnGroundedStateChange => _grounds.ObserveCountChanged().Select(c => c > 0);

        public IObservable<ISet<Collider>> OnGroundContactsChange
        {
            get
            {
                var onAdd = _grounds.ObserveAdd().Select(e => e.Value);
                var onRemove = _grounds.ObserveRemove().Select(e => e.Value);

                return onAdd.Merge(onRemove).Select(_ => (ISet<Collider>) _grounds.ToHashSet());
            }
        }

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

                var oldContacts = GroundContacts.Except(collection);
                var contacts = collection.Except(GroundContacts);

                oldContacts.ForEach(c => _grounds.Remove(c));
                contacts.ForEach(c => _grounds.Add(c));
            }
        }
    }
}