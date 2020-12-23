using UnityEngine;

namespace RPG.Combat
{
    public class Weapons
    {
        public int identifier;
        float weaponDamage;
        int weight;
        int[] noOfCombos;
        public string weaponName;
        float[][] staminaRequirements;
        float[][] attackDamages;
        float[][] poiseDamages;
        Animator anim;
        int inCombo;
        Stats stats;
        bool canMove;
        GameObject weaponObject;
        float waitTime = 0f;

        public Weapons(int identifier, WeaponStats weaponStats , Animator animator,Stats stats, GameObject weaponObject)
        {
            
            this.identifier = identifier;
            weaponDamage = weaponStats.weaponDamage;


            weight = weaponStats.weight;
            noOfCombos = weaponStats.noOfCombos;

            weaponName = weaponStats.weaponName;
            anim = animator;
            this.stats = stats;

            

            staminaRequirements = new float[3][];
            staminaRequirements[0] = weaponStats.oneHandStaminaRequirements;
            staminaRequirements[1] = weaponStats.twoHandStaminaRequirements;
            staminaRequirements[2] = weaponStats.weaponArtStaminaRequirements;


            attackDamages = new float[3][];
            attackDamages[0] = weaponStats.oneHandDamage;
            attackDamages[1] = weaponStats.twoHandDamage;
            attackDamages[2] = weaponStats.weaponArtDamage;

            poiseDamages = new float[3][];
            poiseDamages[0] = weaponStats.oneHandPoiseDamage;
            poiseDamages[1] = weaponStats.twoHandPoiseDamage;
            poiseDamages[2] = weaponStats.weaponArtPoiseDamage;

            this.weaponObject = weaponObject;
        }
        public void Update()
        {
            canMove = anim.GetBool("canMove");
            if (waitTime > 0f) waitTime -= Time.deltaTime;
            if (canMove && waitTime<=0) inCombo = 0;
        }
        public void Attack(bool twoHand, bool weaponArt)
        {

            if (anim.GetBool("nextAttackCombo")) return;
            if (stats.currentStamina <= 0) return;
            int index;
            if (weaponArt) index = 2;
            else if (twoHand) index = 1;
            else index = 0;
            if (inCombo >= noOfCombos[index]) inCombo = 0;

                bool canAttack = anim.GetBool("canAttack");
            bool cancelAnimation = anim.GetBool("cancelAnimation");
            if (canMove)
                inCombo = 0;
            else
            {
                if (canAttack == false) return;
                if (noOfCombos[index] <= inCombo && cancelAnimation)
                {
                    inCombo = 0;
                    anim.CrossFade("empty_override", 0.1f);
                }
            }
            stats.damageMultiplier = weaponDamage;
            anim.SetBool("twoHand", twoHand);
            anim.SetInteger("weaponNo", identifier);
            anim.SetFloat("damage", attackDamages[index][inCombo]);
            anim.SetFloat("poise", poiseDamages[index][inCombo]);
            anim.SetBool("knockBack", false);
            if (inCombo == 0)
            {
                anim.SetTrigger("attack");
            }
            else anim.SetBool("nextAttackCombo",true);
            waitTime = 0.2f;
            stats.reduceStamina(staminaRequirements[index][inCombo]);
            inCombo++;
        }
        public void Activate()
        {
            weaponObject.SetActive(true);
            anim.SetInteger("weaponNo", identifier);
        }
        public void Deactivate()
        {
            weaponObject.SetActive(false);
        }
    }
}