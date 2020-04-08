using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.combat
{
    public class Combat : MonoBehaviour
    {
        [SerializeField] private Collider[] hitDetection;
        [SerializeField] private float weaponDamage = 200f;
        [SerializeField] private float cooldown = 1f;
        [SerializeField] private float dodgeCooldown = 1f;
        public bool invoke = false;

        bool invokedDodge = false;
        bool invokedAttack = false;

        private HashSet<Collider> damage;

        private float timeSinceLastAttack = 0f;
        private float timeSinceLastDodge = 0f;
        private Animator animator;

        private void Start()
        {
            invoke = false;
            animator = GetComponent<Animator>();
            animator.ResetTrigger("attack");
            damage = new HashSet<Collider>();
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            timeSinceLastDodge += Time.deltaTime;
            timeSinceLastAttack = Mathf.Clamp(timeSinceLastAttack, 0, cooldown);
            timeSinceLastDodge = Mathf.Clamp(timeSinceLastDodge, 0, dodgeCooldown);
           

        }


        public void Attack()
        {
            if (timeSinceLastAttack >= cooldown && !IsInAction())
            {
                invokedAttack = false;
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
            else if(invoke) InvokeAttack(Mathf.Max(cooldown - timeSinceLastAttack, dodgeCooldown - timeSinceLastDodge));
            
        }

        public void Dodge()
        {

            if (!IsInAction() && timeSinceLastDodge >= dodgeCooldown)
            {
                invokedDodge = false;
                animator.SetTrigger("dodge");
                timeSinceLastDodge = 0f;
            }
            else InvokeDodge(Mathf.Max(cooldown-timeSinceLastAttack,dodgeCooldown-timeSinceLastDodge));
        }

       
        public Boolean IsAttacking()
        {
            if (cooldown <= timeSinceLastAttack) return false;
            else return true;
            //return animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        }
        public Boolean IsDodging()
        {
            if (dodgeCooldown <= timeSinceLastDodge) return false;
            else return true;
            //return animator.GetCurrentAnimatorStateInfo(0).IsName("dodge");
        }

        public bool IsInAction()
        {
            return IsDodging() || IsAttacking();
        }

        void InvokeDodge(float t)
        {
            if(t<=0.5f && !invokedDodge)
            {
                Invoke("Dodge", t);
                invokedDodge = true;
                Debug.Log("invoked");
            }
        }
        void InvokeAttack(float t)
        {
            if (t <= 0.7f && !invokedAttack)
            {
                Invoke("Attack",t);
                invokedAttack = true;
                Debug.Log("invoked");
            }
        }
    }
}
