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

        bool invokedDodge = false;
        bool invokedAttack = false;

        private HashSet<Collider> damage;

        private float timeSinceLastAttack = 0f;
        private float timeSinceLastDodge = 0f;
        private Animator animator;

        private void Start()
        {
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
            if (IsAttacking())
            {
                AddDamage(hitDetection[0]);
            }
            ;

        }


        public void Attack()
        {
            if (timeSinceLastAttack >= cooldown && !IsInAction())
            {
                invokedAttack = false;
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
            else InvokeAttack(Mathf.Max(cooldown - timeSinceLastAttack, dodgeCooldown - timeSinceLastDodge));
            
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

        private void Hit()
        {

            DoDamage();
            
        }

        public void DoDamage()
        {   
            
            foreach (var d in damage)
            {

                d.transform.parent.parent.GetComponent<Stats>().TakeDamage(weaponDamage);
            }
            damage.Clear();
        }

        public void AddDamage(Collider col)
        {
            
            
            Collider[] cols = Physics.OverlapBox(col.transform.position, col.transform.localScale / 2, col.transform.rotation, LayerMask.GetMask("hitbox"));
            Debug.Log(cols.Length);


            foreach (var c in cols)
            {

                if (c.transform.parent.parent == transform) continue;
                if (c.transform.parent.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("dodge")) 
                {
                    Debug.Log("Dodging");
                    continue;
                }

                damage.Add(c);
            }
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
