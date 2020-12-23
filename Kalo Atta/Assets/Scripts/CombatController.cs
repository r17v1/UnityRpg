using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class CombatController : MonoBehaviour
    {


        public Transform weaponHolder;
        int noOfWeapons;
        private Stats stats;
        private Animator anim;
        private bool twoHand;
        private bool weaponArt;
        private bool attackPressed;
        private List<Weapons> weapons;
        private int currentlyEquipedWeapon;

        public bool isInitialized { get; private set; } = false;

        private void Start()
        {
            
            anim = GetComponent<Animator>();
            stats = GetComponent<Stats>();
            weapons = new List<Weapons>();
            currentlyEquipedWeapon = 0;
        }
        public void Init()
        {
            int index = 0;
            foreach(Transform t in weaponHolder)
            {
                Debug.Log(t.name);
                weapons.Add(new Weapons(index, t.GetComponent<WeaponStats>(), anim, stats, t.gameObject));
                noOfWeapons++;
                index++;
            }
            UpdateEquiped();
            isInitialized = true;
        }

        public void Tick(bool attackPressed, bool twoHand,bool weaponArt,bool previousWeapon, bool nextWeapon)
        {
            weapons[currentlyEquipedWeapon].Update();
            this.weaponArt = weaponArt;
            this.attackPressed = attackPressed;
            if (weaponArt) this.attackPressed = true;
            if (twoHand) this.twoHand = !this.twoHand;
            if (previousWeapon) currentlyEquipedWeapon--;
            if (currentlyEquipedWeapon < 0) currentlyEquipedWeapon = noOfWeapons - 1;
            else if (nextWeapon) currentlyEquipedWeapon =(currentlyEquipedWeapon+1)%noOfWeapons;
            if (previousWeapon || nextWeapon) UpdateEquiped();
            anim.SetBool("twoHand", this.twoHand);
            TryAttack();

        }

        void UpdateEquiped()
        {
            for(int i=0;i<noOfWeapons;i++)
            {
                if (i == currentlyEquipedWeapon)
                {
                    weapons[i].Activate();
                    Debug.Log("activated weapon" + weapons[i].weaponName);
                }
                else weapons[i].Deactivate();
            }
        }
    
        private void TryAttack()
        {
            if (attackPressed == false) return;
            weapons[currentlyEquipedWeapon].Attack(twoHand,weaponArt);

        }

    }
}