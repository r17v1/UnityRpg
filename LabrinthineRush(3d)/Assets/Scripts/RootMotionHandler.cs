using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  RPG.Controller;

namespace AnimationHelper
{
    public class RootMotionHandler : MonoBehaviour
    {


        void OnAnimatorMove()

        {
            Animator anim = GetComponent<Animator>();


            if (!anim.GetBool("canMove"))
            {

                anim.ApplyBuiltinRootMotion();

            }


        }
    }
}
