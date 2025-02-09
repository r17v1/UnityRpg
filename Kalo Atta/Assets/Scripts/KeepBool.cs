﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBool : StateMachineBehaviour
{
    public string boolName;
    public bool status;
    public bool reset;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (reset)
            animator.SetBool(boolName, !status);
        else animator.SetBool(boolName, status);
    }
}
