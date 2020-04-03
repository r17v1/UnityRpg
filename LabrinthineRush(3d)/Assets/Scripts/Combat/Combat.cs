using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.combat
{
    public class Combat : MonoBehaviour
    {
        [SerializeField] private Collider[] hitDetection;
        [SerializeField] private float weaponDamage = 200f;
        [SerializeField] private float cooldown = .8f;
        private float timeSinceLastAttack = 0f;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.ResetTrigger("attack");
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            timeSinceLastAttack = Mathf.Clamp(timeSinceLastAttack, 0, cooldown);
        }


        public void Attack()
        {
            if (timeSinceLastAttack >= cooldown)
            {
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
            
        }

        private void Hit()
        {
            DoDamage(hitDetection[0]);
        }

        private void DoDamage(Collider col)
        {
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("hitbox"));
            foreach (var c in cols)
            {
                if (c.transform.parent.parent == transform) continue;
                Debug.Log(c.name);
                c.transform.parent.parent.GetComponent<Stats>().TakeDamage(weaponDamage);
            }
        }

        public Boolean IsAttacking()
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        }
    }
}
