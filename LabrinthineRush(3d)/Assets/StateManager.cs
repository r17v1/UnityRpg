using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateManager : MonoBehaviour
{
    public float vertical;
    public float horizontal;

    //[HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public float delta;

    [HideInInspector]
    Transform camera;

    bool canMove = true;

    public bool L1Attack=false;

    public Vector3 moveDirection;
    public float moveSpeed = 3f; 
    public float runSpeed = 5f;
    public float moveAmount;
    public float distanceToGround;
    public bool lockOn = false;
    public bool twoHand = false;
    string currentAttack;
    float smoothVertical = 0;
    float smoothHorizontal = 0;

    bool stop = false;

    public bool dodge;


    public bool run;

    public void Init()
    {
        anim = GetComponentInChildren<Animator>();
        //Debug.Log("animator=" + anim);
        rigid = GetComponent<Rigidbody>();
        camera = Camera.main.transform;
        rigid.angularDrag = 999f;
        rigid.drag = 4;
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }
    public void Tick(float t)
    {
        
        
        if (twoHand) currentAttack = "th_attack_1";
        else currentAttack = "oh_attack_1";
        canMove = anim.GetBool("canMove");
        /*if (stop)
        {
            
            rigid.velocity = Vector3.zero;
            stop = false;
            return; 
        }*/
        if (!canMove) return;
        
        
        Debug.Log("grounded"+isGrounded());
        delta = t;
        Rotate();
        rigid.drag= (moveAmount>0 || isGrounded()==false)? 0f:4f;
        float speed = run ? runSpeed : moveSpeed;
        if(isGrounded())
            rigid.velocity = moveDirection * speed;
        HandleMovementAnimations();
        
    }
    void HandleMovementAnimations()
    {
        if (!isGrounded()) return;
        if (!canMove) return;
        anim.SetBool("twoHand", twoHand);

        if (lockOn)
        {
            
            if(run)
            {
                smoothHorizontal = FloatLerp(smoothHorizontal, horizontal, 6 * Time.deltaTime);
                smoothVertical = FloatLerp(smoothVertical, vertical, 6 * Time.deltaTime);
            }
            else
            {
                smoothHorizontal = FloatLerp(smoothHorizontal, horizontal/2f, 4 * delta);
                smoothVertical = FloatLerp(smoothVertical, vertical/2f, 4 * delta);
            }

            anim.SetFloat("rollV", MyCeil(vertical,0.3f));
            anim.SetFloat("rollH", MyCeil(horizontal,0.3f));
            anim.SetFloat("vertical", smoothVertical);
            anim.SetFloat("horizontal",smoothHorizontal);
            anim.SetBool("lockOn", true);

        }
        else
        {
            
            anim.SetFloat("vertical", moveAmount);
            anim.SetFloat("rollV", MyCeil(moveAmount,0.3f));
            anim.SetFloat("rollH", 0f);
            anim.SetFloat("horizontal", 0f);
            anim.SetBool("lockOn", false);
        }
       
        if (L1Attack) { anim.CrossFade(currentAttack, 0.2f); stop = true; }
        if (dodge)
        {

            stop = true;
            anim.CrossFade("dodge", 0.2f);
        }
        
    }
    bool isGrounded()
    {
        bool r = false;
        var origin = transform.position + Vector3.up * 0.5f;
        var dir = -Vector3.up;
        float dist = 0.8f;
        RaycastHit hit;
        if(Physics.Raycast(origin,dir,out hit,dist))
        {
            r = true;
            var targetPos = hit.point;
            transform.position = targetPos;

        }
        return r;

        //return Physics.Raycast(GetComponent<CapsuleCollider>().center, -Vector3.up, GetComponent<CapsuleCollider>().height + 0.1f );
    }

    void Rotate()
    {
        if(lockOn)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, camera.rotation, delta * 5);
            return;
        }

        
        if (Mathf.Abs(vertical) < 0.01f && Mathf.Abs(horizontal) < 0.01f && !lockOn) return;
        var angle = Mathf.Atan2(horizontal, vertical);
        angle = Mathf.Rad2Deg * angle;
        angle += Camera.main.transform.eulerAngles.y;

        var targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, delta * 5);
    }


    float MyCeil(float f,float tolerance)
    {
        if (f > 0)
        {
            if (f > tolerance) return 1;
            else return 0;
        }
        else
        {
            if (f < -1f*tolerance) return -1;
            else return 0;
        }
    }

    float FloatLerp(float initialValue, float finalValue, float speed)
    {
        if (initialValue > finalValue) speed = -speed;

        float r = initialValue;
        r += speed;
        return Mathf.Clamp(r, Mathf.Min(initialValue, finalValue), Mathf.Max(initialValue, finalValue));
    }

}

