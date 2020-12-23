using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class CallDamage : MonoBehaviour
    {
        public Transform weaponHolder;
        DamageOnCollision col;
        Animator anim;
        Stats stats;
        private void Start()
        {
            anim = GetComponent<Animator>();
            stats = GetComponent<Stats>();
        }
        public void OpenDamageColliders()
        {
            foreach (Transform t in weaponHolder)
            {
                if (t.gameObject.activeSelf == true)
                {
                    col = t.GetComponent<DamageOnCollision>();
                    break;
                }
            }
            col.weaponDamage = anim.GetFloat("damage")*stats.damageMultiplier;
            col.poiseDamage = anim.GetFloat("poise");
            col.knockBack = anim.GetBool("knockBack");
            col.OpenDamageColliders();
            anim.SetBool("canAttack", true);
        }
        public 
        void CloseDamageColliders()
        {
            foreach (Transform t in weaponHolder)
            {
                if (t.gameObject.activeSelf == true)
                {
                    col = t.GetComponent<DamageOnCollision>();
                    break;
                }
            }
            Debug.Log(col.name);
            col.CloseDamageColliders();
        }
        void CancelAnimation()
        {
            anim.SetBool("cancelAnimation", true);
        }
    }
}