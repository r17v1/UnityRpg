using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class collisionMove : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<NavMeshAgent>().enabled = false;
        Debug.Log("collision enter");
    }
    private void OnCollisionExit(Collision collision)
    {
        GetComponent<NavMeshAgent>().enabled = true;
        Debug.Log("collision exit");
    }
}
