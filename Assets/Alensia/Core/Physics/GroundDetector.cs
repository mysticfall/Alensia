using Alensia.Core.Common;
using UnityEngine;

namespace Alensia.Core.Physics
{
    [RequireComponent(typeof(Collider))]
    public class GroundDetector : MonoBehaviour
    {
        public Collider Collider { get; private set; }

        public bool Grounded { get; private set; }

        public void Start()
        {
            //Collider = this.GetComponentOrFail<Collider>();
        }

        public void OnCollisionStay(Collision collisionInfo)
        {
            //if (center - extent >= y)
            if (!Grounded && collisionInfo.contacts.Length > 0)
            {
                Grounded = true;

                SendMessage("OnHitGround");
            }
        }

        public void OnCollisionExit(Collision collisionInfo)
        {
            if (Grounded)
            {
                var center = Collider.bounds.center.y;
                var extent = Collider.bounds.extents.y;

                var inAir = collisionInfo.contacts.Length == 0;

                if (inAir)
                {
                    Grounded = false;

                    SendMessage("OnLeaveGround");
                }
            }
        }
    }
}