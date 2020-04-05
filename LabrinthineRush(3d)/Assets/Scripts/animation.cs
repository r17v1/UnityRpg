using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAnimatorMove()

    {
        Debug.Log("called0");
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // APPLY DEFAULT ROOT MOTION, ONLY WHEN IN THESE ANIMATION STATES
        if (stateInfo.IsName("dodge"))
        {
            Debug.Log("called1");
            anim.ApplyBuiltinRootMotion();
        }
    }
}
