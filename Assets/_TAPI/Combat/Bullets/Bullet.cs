using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TAPI.Combat.Bullets
{
    /// <summary>
    /// A bullet object. This handles the speed and position of the bullet.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public Vector3 Speed { get { return speed; } }
        public Vector3 LocalSpeed { get { return localSpeed; } }

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Vector3 speed;
        [SerializeField] private Vector3 localSpeed;
        [SerializeField] private Vector3 angularSpeed;
        [SerializeField] private Vector3 localAngularSpeed;
        
        public void Tick()
        {
            if(angularSpeed != Vector3.zero)
            {
                transform.Rotate(angularSpeed, Space.World);
            }
            if (localAngularSpeed != Vector3.zero)
            {
                transform.Rotate(localAngularSpeed, Space.Self);
            }
            rigidbody.velocity = GetForwardBasedSpeed(localSpeed)
                + speed;
        }

        public void SetPosition(Vector3 position)
        {
            rigidbody.position = position;
        }

        public void SetSpeed(Vector3 speed)
        {
            this.speed = speed;
        }

        public void AddSpeed(Vector3 speed)
        {
            this.speed += speed;
        }

        public void SetLocalSpeed(Vector3 speed)
        {
            this.localSpeed = speed;
        }

        public void AddLocalSpeed(Vector3 speed)
        {
            this.localSpeed += speed;
        }

        public void SetAngularSpeed(Vector3 angularSpeed)
        {
            this.angularSpeed = angularSpeed;
        }

        public void AddAngularSpeed(Vector3 angularSpeed)
        {
            this.angularSpeed += angularSpeed;
        }

        public void SetLocalAngularVelocity(Vector3 angularVelocity)
        {
            this.localAngularSpeed = angularVelocity;
        }

        public void AddLocalAngularVelocity(Vector3 angularVelocity)
        {
            this.localAngularSpeed += angularVelocity;
        }

        public Vector3 GetForwardBasedSpeed(Vector3 speed)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            return forward * speed.z + 
                right * speed.x 
                + up * speed.y;
        }
    }
}
