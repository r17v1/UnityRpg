using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDamage : StateMachineBehaviour
{

    public string floatName;
    public float value;
    public bool reset;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(floatName, value);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (reset)
            animator.SetFloat(floatName, 0f);
    }
}
