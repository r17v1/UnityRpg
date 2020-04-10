using UnityEngine;
using UnityEngine;
using RPG.Helper;
using System;
using RPG.Movement;
using RPG.Combat;
using RPG.UI;

namespace RPG.Controller
{
    public class InputHandler : MonoBehaviour
    {
        // Start is called before the first frame update

        public HealthBar health;
        public StaminaBar stamina;
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
            sprint = false;
            health.setMaxHealth(GetComponent<Stats>().health);
            stamina.setMaxStamina(GetComponent<Stats>().stamina);

        }

        private void Update()
        {
           
            getInput();
            UpdatePlayerAction();
            action.Tick();
            cc.Tick();
            UpdateUI();
        }
        void UpdateUI()
        {
            health.setHealth(GetComponent<Stats>().currentHealth);
            stamina.setStamina(GetComponent<Stats>().currentStamina);
        }

        void getInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            attack = Input.GetButtonDown("Attack");
            dodge = Input.GetButtonDown("Dodge");

            if( Input.GetButtonDown("Sprint"))sprint=true;
            if (Input.GetButtonUp("Sprint")) sprint = false;

            cameraLock = Input.GetButtonDown("Camera Lock");
            twoHand = Input.GetButtonDown("Two Hand");
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
            if (action.stats.currentStamina <= 0) { action.run = false; sprint = false; }

            var v = new Vector3(camera.forward.x, 0, camera.forward.z).normalized * vertical;
            var h = new Vector3(camera.right.x, 0, camera.right.z).normalized * horizontal;
            action.moveDirection = (v + h).normalized;

            if (action.moveDirection.magnitude > 0)
            {
                if (action.run) { action.moveAmount = MyFloat.FloatLerp(action.moveAmount, 1f, action.deltaTime * 4); }
                else action.moveAmount = MyFloat.FloatLerp(action.moveAmount, 0.6f, action.deltaTime * 4);
            }
            else action.moveAmount = MyFloat.FloatLerp(action.moveAmount, 0f, action.deltaTime * 4);
            action.attack = attack;
            if (twoHand) action.twoHand = !action.twoHand;
            action.dodge = dodge;
            
        }


        void lockOn()
        {
            Collider[] array = Physics.OverlapSphere(transform.position, 1000f, lockLayer);
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
    }
}