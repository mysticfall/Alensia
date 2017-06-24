using System.Collections.Generic;
using System.Linq;
using Alensia.Core.Common;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Alensia.Core.Physics
{
    public class CollisionBasedGroundDetector : GroundDetector
    {
        public override GroundDetectionSettings Settings { get; }

        public override Collider Target { get; }

        public CollisionBasedGroundDetector(Collider target) : 
            this(new GroundDetectionSettings(), target)
        {
        }

        [Inject]
        public CollisionBasedGroundDetector(
            GroundDetectionSettings settings, Collider target)
        {
            Assert.IsNotNull(settings, "settings != null");
            Assert.IsNotNull(target, "target != null");

            Settings = settings;
            Target = target;

            target.OnCollisionEnterAsObservable().Subscribe(OnCollisionEnter).AddTo(this);
            target.OnCollisionExitAsObservable().Subscribe(OnCollisionExit).AddTo(this);
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
                where c.point.y <= bottom + Settings.Tolerance && IsGround(c.otherCollider)
                select c.otherCollider;

            return grounds;
        }
    }
}