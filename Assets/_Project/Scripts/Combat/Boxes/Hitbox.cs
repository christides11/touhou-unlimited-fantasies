using UnityEngine;

namespace TUF.Combat
{
    public class Hitbox : CAF.Combat.Hitbox
    {
        [SerializeField] protected GameObject rectangleVisual;
        [SerializeField] protected GameObject sphereVisual;
        [SerializeField] protected GameObject capsuleVisual;

        protected override void CreateRectangle(Vector3 size)
        {
            base.CreateRectangle(size);
            rectangleVisual.transform.localScale = size;
            rectangleVisual.SetActive(true);
        }

        protected override void CreateSphere(float radius)
        {
            base.CreateSphere(radius);
            sphereVisual.transform.localScale = Vector3.one * radius;
            sphereVisual.SetActive(true);
        }

        protected override void CreateCapsule(float radius, float height)
        {
            base.CreateCapsule(radius, height);
            capsuleVisual.transform.localScale = new Vector3(1, height, 1);
            capsuleVisual.SetActive(true);
        }

        public override void SimLateUpdate()
        {
            base.SimLateUpdate();
        }
    }
}