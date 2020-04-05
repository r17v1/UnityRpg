using UnityEngine;


namespace RPG.combat
{
    
    public class Stats : MonoBehaviour
    {
        public float health = 100f;
        public float defence= 10f;
        public bool death = false;

        // Update is called once per frame
        public void Update()
        {
            if (death) return;
            if (health > 100f) health = 100f;
            if (health < 0f) health = 0f;
            if (health == 0f) Die();
            
        }
        public bool TakeDamage(float damage)
        {
            //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("dodge")) return false;
            
            health -= damage / defence;
            Debug.Log(health);
            if (health <= 0) return true;
            return false;
        }
        public void Die()
        {
            GetComponent<Animator>().SetTrigger("death");
            death = true;
            Destroy(gameObject, 5);
        }
    }
}