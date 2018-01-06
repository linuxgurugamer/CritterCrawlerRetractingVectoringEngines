using System;
using UnityEngine;


namespace CritterCrawler
{
    public class CritterCrawler : PartModule
    {
        [KSPField(isPersistant = false)]
        double maxSpeed = 10f;

        [KSPField(isPersistant = false, guiActive = true, guiName = "Sticky Feet")]
        public bool stickyFeet = false;


        [KSPEvent(guiActive = true, guiName = "Toggle Sticky Feet", active = true)]
        public void toggleStickyFeet()
        {
            stickyFeet = !stickyFeet;
        }


        public override void OnUpdate()
        {
            //downforce
            if (stickyFeet && vessel.Landed)
            {
                Rigidbody rigidBody = vessel.rootPart.rb;
                if (rigidBody == null)
                {
                    Debug.Log("rigidBody is null");
                    return;
                }

                UnityEngine.RaycastHit hitInfo = new UnityEngine.RaycastHit();
                var mask = 1 << 15;
                if (Physics.Raycast(this.part.transform.position, -this.part.transform.up, out hitInfo, 1.4f, mask))
                {
                    //this.part.rigidbody.AddForce(-15*this.part.transform.up);
                    rigidBody.AddForceAtPosition(-15 * this.part.transform.up, this.vessel.CoM);

                    //this.part.rigidbody.AddRelativeTorque(-20 * this.vessel.angularMomentum);
                    rigidBody.AddRelativeTorque(-20 * this.vessel.angularMomentum);
                }

                if (vessel.Landed)
                {
                    if (vessel.srf_velocity.magnitude > maxSpeed)
                    {
                        rigidBody.AddForce(-2 * rigidBody.velocity);
                    }
                    if (vessel.ctrlState.wheelThrottle == 0)
                    {
                        rigidBody.AddForce(-2 * rigidBody.velocity);
                    }
                }
            }
        }
    }
}

