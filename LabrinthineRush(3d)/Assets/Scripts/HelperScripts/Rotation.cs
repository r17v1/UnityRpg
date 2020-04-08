using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Helper
{
    public class Rotation : MonoBehaviour
    {
       public static Quaternion LookAtY(Vector3 transformToRotate, Vector3 target)
        {
            Vector3 look = new Vector3(target.x, transformToRotate.y, target.z);
            //transformToRotate.LookAt(look);

            var targetRotation = Quaternion.LookRotation(look - transformToRotate);

            return targetRotation;

        }
        public static Quaternion LookAt(Vector3 transformToRotate, Vector3 target)
        {
            Vector3 look = new Vector3(target.x, target.y, target.z);
            //transformToRotate.LookAt(look);

            var targetRotation = Quaternion.LookRotation(look - transformToRotate);

            return targetRotation;

        }
    }

    
}

