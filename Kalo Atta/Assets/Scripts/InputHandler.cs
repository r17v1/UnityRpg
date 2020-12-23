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
        bool nextWeapon;
        bool previousWeapon;
        CombatController combat;
        public LayerMask lockLayer;

        //StateManager state;

        MovementController move;
        CameraControl cameraControl;

        void Start()
        {
            move = GetComponent<MovementController>();
            cameraControl = Camera.main.transform.GetComponent<CameraControl>();
            cameraControl.Init(transform);
            sprint = false;
            health.setMaxHealth(GetComponent<Stats>().health);
            stamina.setMaxStamina(GetComponent<Stats>().stamina);
            combat = GetComponent<CombatController>();
        }

        private void Update()
        {

            
            if (combat.isInitialized == false) combat.Init();
            getInput();
            combat.Tick(attack, twoHand, false,previousWeapon,nextWeapon);
            

            move.Tick(vertical, horizontal, sprint, dodge, Time.deltaTime);

            cameraControl.Tick();
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
            if (cameraLock) LockOn();
            twoHand = Input.GetButtonDown("Two Hand");
            nextWeapon = Input.GetButtonDown("Next Weapon");
            previousWeapon = Input.GetButtonDown("Previous Weapon");
        }


        void LockOn()
        {
            Collider[] array = Physics.OverlapSphere(transform.position, 1000f, lockLayer);
            if (array.Length > 1 && Camera.main.transform.GetComponent<CameraControl>().lockOn == null)
            {
                Array.Sort(array, new RPG.Helper.Comparer(transform));
                cameraLock = true;

                Camera.main.transform.GetComponent<CameraControl>().lockOn = array[1].transform;
                Debug.Log("lockOn");
            }
            else
            {
                Camera.main.transform.GetComponent<CameraControl>().lockOn = null;
                cameraLock = false;
            }
        }
    }
}