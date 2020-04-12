using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class DamageOnCollision : MonoBehaviour
    {
        bool damageCollider = false;
        public string tag = "enemy";

        public bool knockBack;
        public float weaponDamage = 200f;
        public float poiseDamage = 20f;

        public void OpenDamageColliders()
        {
            damageCollider = true;
        }
        public void CloseDamageColliders()
        {
            damageCollider = false;

        }

        private void OnTriggerEnter(Collider other)
        {



            if (other.gameObject.tag != tag) return;
            if (damageCollider)
            {
                Debug.Log("WD" + weaponDamage + "PD" + poiseDamage + "KB" + knockBack);
                other.transform.GetComponentInParent<Stats>().TakeDamage(weaponDamage, poiseDamage, knockBack); 
            }

        }
    }
}
