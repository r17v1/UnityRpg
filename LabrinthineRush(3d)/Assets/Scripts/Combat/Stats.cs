using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.combat
{
    
    public class Stats : MonoBehaviour
    {
        public float health = 100f;
        public float defence= 10f;
   

        // Update is called once per frame
        void Update()
        {
            if (health > 100f) health = 100f;
            if (health < 0f) health = 0f;
            
        }
        public bool TakeDamage(float damage)
        {
            health -= damage / defence;
            Debug.Log(health);
            if (health <= 0) return true;
            return false;
        }
    }
}