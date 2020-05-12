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

        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Vector3 speed;

        public void Tick()
        {
            rigidbody.velocity = speed;
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

        public Vector3 GetForwardBasedSpeed(Vector3 speed)
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            return forward * speed.z + right * speed.x + up * speed.y;
        }
    }
}
