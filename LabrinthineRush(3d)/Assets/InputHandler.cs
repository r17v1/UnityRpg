using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.camera;
using RPG.Helper;
using System;


namespace RPG.movement
{
    public class InputHandler : MonoBehaviour
    {
        // Start is called before the first frame update

        float horizontal;
        float vertical;
        bool attack;
        bool dodge;
        bool block;
        bool sprint;
        bool cameraLock;
        bool twoHand;

        public LayerMask lockLayer;

               //StateManager state;

        PlayerMovement action;
    CameraControl cc;
    Transform camera;

        void Start()
        {
            action = GetComponent<PlayerMovement>();
            camera = Camera.main.transform;
            cc = camera.GetComponent<CameraControl>();
            cc.Init(transform);

        }


        void FixedUpdate()
        {

            

        }
        private void Update()
        {
           
            //Debug.Log("button: " + Input.GetButtonDown("Fire1"));
            getInput();
            UpdatePlayerAction();
            action.Tick();
            cc.Tick();
        }
        public void LateUpdate()
        {
            
        }

        void getInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            attack = Input.GetButtonDown("Attack");
            dodge = Input.GetButtonDown("Dodge");
            sprint = Input.GetButton("Sprint");
            cameraLock = Input.GetButtonDown("Camera Lock");
            twoHand = Input.GetButtonDown("Two Hand");

            Debug.Log("dodge" + dodge);
        }


        void UpdatePlayerAction()
        {
            if (camera.GetComponent<CameraControl>().lockOn == null) action.lockOn = false;
            action.deltaTime = Time.deltaTime;
            if (cameraLock)
                lockOn();
            action.vertical = vertical;
            action.horizontal = horizontal;
            action.run = sprint;
            
            var v = new Vector3(camera.forward.x, 0, camera.forward.z).normalized * vertical;
            var h = new Vector3(camera.right.x, 0, camera.right.z).normalized * horizontal;
            action.moveDirection = (v + h).normalized;

            if (action.moveDirection.magnitude > 0)
            {
                if (action.run) { action.moveAmount = FloatLerp(action.moveAmount, 1f, action.deltaTime * 4); }
                else action.moveAmount = FloatLerp(action.moveAmount, 0.6f, action.deltaTime * 4);
            }
            else action.moveAmount = FloatLerp(action.moveAmount, 0f, action.deltaTime * 4);
            action.attack = attack;
            if (twoHand) action.twoHand = !action.twoHand;
            action.dodge = dodge;
            
        }




        void lockOn()
        {


            // LayerMask layer = LayerMask.GetMask("enemy");
            Collider[] array = Physics.OverlapSphere(transform.position, 1000f, lockLayer);
            Debug.Log(array.Length);
            if (array.Length > 1 && Camera.main.transform.GetComponent<CameraControl>().lockOn == null)
            {
                Array.Sort(array, new RPG.Helper.Comparer(transform));
                action.lockOn = true;

                Camera.main.transform.GetComponent<CameraControl>().lockOn = array[1].transform;


            }
            else
            {
                Camera.main.transform.GetComponent<CameraControl>().lockOn = null;
                action.lockOn = false;

            }
        }



        float FloatLerp(float initialValue, float finalValue, float speed)
        {
            if (initialValue < finalValue) speed = -speed;

            float r = initialValue;
            r -= speed;
            return Mathf.Clamp(r, Mathf.Min(initialValue, finalValue), Mathf.Max(initialValue, finalValue));
        }
    }
}