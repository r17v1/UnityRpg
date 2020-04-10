using UnityEngine;
using RPG.UI;

namespace RPG.Combat
{
    
    public class Stats : MonoBehaviour
    {
        //public HealthBar healthBar;
        //public StaminaBar staminaBar;
        public float health = 100f;
        public float stamina = 100f;
        public float staminaRegenSpeed = 10f;
        public float currentHealth;
        public float currentStamina;
        public float defence= 10f;
        public float poise = 30;
        public float currentPoise;

        public float staminaRegenCooldownTime = 1f;
        public float staminaRegenCooldown = 0f;
        public float poiseRegenSpeed = 5f;
        public float damageMultiplier = 10f;

        public bool death = false;
        public bool canMove = true;
        public bool sprinting = false;

        public bool invulnerable;

        public Animator anim;

        private void Start()
        {
            currentStamina = stamina;
            currentHealth = health;
            anim = GetComponent<Animator>();
            currentPoise = poise;
            //healthBar.setMaxHealth(health);
            //staminaBar.setMaxStamina(stamina);
        }
        public void Update()
        {

            //healthBar.setHealth(currentHealth);
            //staminaBar.setStamina(currentStamina);
            if (death) return;
            if (currentHealth > health)currentHealth = health;
            if (currentHealth < 0f) currentHealth = 0f;
            if (currentHealth == 0f) Die();
            updateBools();
            if (sprinting == false && canMove == true)
            {
                if (staminaRegenCooldown > 0)
                {
                    staminaRegenCooldown -= Time.deltaTime;
                }
                else regenStamina();
            }
            else staminaRegenCooldown = staminaRegenCooldownTime;
            canMove = anim.GetBool("canMove");
            invulnerable = anim.GetBool("dodging");
            regenPoise();

            
            
        }
        public bool TakeDamage(float damage,float poiseDamage,bool knockBack)
        {
            if (invulnerable) return false;
            currentPoise -= poiseDamage;
            if ((canMove || currentPoise <= 0) && !knockBack) 
            {
                anim.CrossFade("damage_1", 0.2f);
            }
            if (knockBack)
                anim.CrossFade("damage_3", 0.1f);
            currentHealth -= damage / defence;            
            if (currentHealth <= 0) return true;
            return false;
        }
        public void Die()
        {
            GetComponent<Animator>().CrossFade("death",0.2f);
            death = true;
            Destroy(gameObject, 5);
        }

        public void updateBools()
        {
            var anim = GetComponent<Animator>();
            canMove = anim.GetBool("canMove");
            sprinting = anim.GetFloat("vertical") >= 1.0f || anim.GetFloat("horizontal") >= 1.0f;
        }

        public void regenStamina()
        {
            currentStamina += staminaRegenSpeed * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, -3, stamina);
        }
        public void regenPoise()
        {
            //if (poise <= 0) currentPoise = poise;
            if (canMove)
                currentPoise += poiseRegenSpeed * Time.deltaTime;
            currentPoise = Mathf.Clamp(currentPoise, 0, poise);
        }

        public void reduceStamina(float s)
        {
            currentStamina -= s;
        }
        
    }
}