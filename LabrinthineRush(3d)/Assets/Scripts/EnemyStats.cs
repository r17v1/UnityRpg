using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyStats : Stats
    {
        public Vector3 initialPosition;
        private void Start()
        {
            initialPosition = transform.position;
            currentStamina = stamina;
            currentHealth = health;
            anim = GetComponent<Animator>();
            currentPoise = poise;
        }
    }
}