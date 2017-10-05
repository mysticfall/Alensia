using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionBasedGroundDetector : GroundDetector
    {
        [Inject]
        public override Collider Target { get; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Target.OnCollisionEnterAsObservable()
                .Subscribe(OnCollisionEnter)
                .AddTo(this);
            Target.OnCollisionExitAsObservable()
                .Subscribe(OnCollisionExit)
                .AddTo(this);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnDetectGround(FindGroundContacts(collision.contacts));
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            OnDetectGround(FindGroundContacts(collision.contacts));
        }

        protected virtual IEnumerable<Collider> FindGroundContacts(
            IEnumerable<ContactPoint> contacts)
        {
            var center = Target.bounds.center.y;
            var extent = Target.bounds.extents.y;

            var bottom = center - extent;

            var grounds = from c in contacts
                where c.point.y <= bottom + Tolerance && IsGround(c.otherCollider)
                select c.otherCollider;

            return grounds;
        }
    }
}