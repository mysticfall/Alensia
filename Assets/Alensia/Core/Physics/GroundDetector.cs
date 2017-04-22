using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Alensia.Core.Physics
{
    public abstract class GroundDetector : IGroundDetector, IDisposable
    {
        public abstract GroundDetectionSettings Settings { get; }

        public abstract Collider Target { get; }

        public Collider Ground { get; private set; }

        public bool Grounded
        {
            get { return Ground; }
        }

        public GroundHitEvent GroundHit { get; private set; }

        public GroundLeaveEvent GroundLeft { get; private set; }

        protected GroundDetector(GroundHitEvent groundHit, GroundLeaveEvent groundLeft)
        {
            Assert.IsNotNull(groundHit, "groundHit != null");
            Assert.IsNotNull(groundLeft, "groundLeft != null");

            GroundHit = groundHit;
            GroundLeft = groundLeft;
        }

        public virtual void Dispose()
        {
            Ground = null;
        }

        protected virtual bool IsGround(Collider collider)
        {
            return true;
        }

        protected void OnDetectGround(Collider ground)
        {
            var oldGround = Ground;

            Ground = ground;

            if (!oldGround && ground)
            {
                GroundHit.Fire(Ground);
            } else if (oldGround && !ground)
            {
                GroundLeft.Fire(oldGround);
            }
        }
    }
}