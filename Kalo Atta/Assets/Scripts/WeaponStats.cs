using UnityEngine;


namespace RPG.Combat
{
    public class WeaponStats : MonoBehaviour
    {
        public float weaponDamage;
        public int[] noOfCombos;
        public int weight;
        public string weaponName;
        public float[] oneHandStaminaRequirements;
        public float[] twoHandStaminaRequirements;
        public float[] weaponArtStaminaRequirements;

        public float[] oneHandDamage;
        public float[] twoHandDamage;
        public float[] weaponArtDamage;

        public float[] oneHandPoiseDamage;
        public float[] twoHandPoiseDamage;
        public float[] weaponArtPoiseDamage;
    }
}