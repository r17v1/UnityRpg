using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CallDamage : MonoBehaviour
    {
        public Transform sword;
        DamageOnCollision col;
        Animator anim;
        Stats stats;
        private void Start()
        {
            anim = GetComponent<Animator>();
            col = sword.GetComponent<DamageOnCollision>();
            stats = GetComponent<Stats>();
        }
        public void OpenDamageColliders()
        {

            
            col.weaponDamage = anim.GetFloat("damage")*stats.damageMultiplier;
            col.poiseDamage = anim.GetFloat("poise");
            col.knockBack = anim.GetBool("knockBack");
            col.OpenDamageColliders();
            anim.SetBool("canAttack", true);
        }
        public 
        void CloseDamageColliders()
        {
            sword.GetComponent<DamageOnCollision>().CloseDamageColliders();
        }
        void CancelAnimation()
        {
            anim.SetBool("cancelAnimation", true);
        }
    }
}