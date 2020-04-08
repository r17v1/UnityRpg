using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  RPG.camera;
public class rootMotionHandler : MonoBehaviour
{
    

    void OnAnimatorMove()

    {
        Animator anim = GetComponent<Animator>();
        

        if (!anim.GetBool("canMove"))
        {

            anim.ApplyBuiltinRootMotion();

            if (Camera.main.transform.GetComponent<CameraControl>().lockOn != null)
            transform.LookAt(Camera.main.transform.GetComponent<CameraControl>().lockOn);
        }
        
        
    }
}
