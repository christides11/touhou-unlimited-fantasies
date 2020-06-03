using UnityEngine;

namespace TUF.Combat
{
    public class Hitbox : CAF.Combat.Hitbox
    {
        [SerializeField] protected GameObject rectangleVisual;
        [SerializeField] protected GameObject sphereVisual;

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
    }
}