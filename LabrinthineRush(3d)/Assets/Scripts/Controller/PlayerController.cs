using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RPG.Helper;
using RPG.combat;
using RPG.movement;
using RPG.camera;

namespace RPG.controller
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMovement move;
        Combat combat;
        Stats stats;
        Animator anim;
        CharacterController charControl;

        public float targetRadious;
        public LayerMask layer;
        


        void Start()
        {
            move = GetComponent<PlayerMovement>();
            combat = GetComponent<Combat>();
            stats = GetComponent<Stats>();
            anim = GetComponent<Animator>();
            charControl = GetComponent<CharacterController>();
            combat.invoke = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (stats.death == true) return;
            if (!combat.IsInAction() || !charControl.isGrounded)
                move.Move();
            if (Input.GetKeyDown(KeyCode.Mouse0) && charControl.isGrounded)
                combat.Attack();
            if(Input.GetKeyDown(KeyCode.Mouse2))
            {
                Collider[] array = Physics.OverlapSphere(transform.position, 1000f, layer);
                Debug.Log(array.Length);
                if (array.Length >1 && Camera.main.transform.GetComponent<CameraControl>().lockOn==null)
                {
                    Array.Sort(array, new Helper.Comparer(transform));

                    Camera.main.transform.GetComponent<CameraControl>().lockOn = array[1].transform;
                    move.lockOn = true;
                }
                else
                {
                    Camera.main.transform.GetComponent<CameraControl>().lockOn = null;
                    move.lockOn = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                combat.Dodge();
                
            }
        }

       
       

    }
    
}