using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.combat
{
    public class EnemyStats : Stats
    {
        public Vector3 initialPosition;
        private void Start()
        {
            initialPosition = transform.position;
        }
    }
}