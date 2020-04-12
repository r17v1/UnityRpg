using System.Collections;

using UnityEngine;

namespace RPG.Helper
{
    public class Comparer : IComparer
    {
        private Transform compareTransform;
        public Comparer(Transform transform)
        {
            compareTransform = transform;
        }

        public int Compare(object x, object y)
        {
            Collider xCollider = x as Collider;
            Collider yCollider = y as Collider;

            Vector3 offset = xCollider.transform.position - compareTransform.position;
            float xDistance = offset.sqrMagnitude;

            offset = yCollider.transform.position - compareTransform.position;
            float yDistance = offset.sqrMagnitude;

            return xDistance.CompareTo(yDistance);
        }
    }
}
