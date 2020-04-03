using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.combat;
using RPG.movement;

namespace RPG.controller
{
    public class PlayerController : MonoBehaviour
    {
        PlayerMovement move;
        Combat combat;

        void Start()
        {
            move = GetComponent<PlayerMovement>();
            combat = GetComponent<Combat>();
        }

        // Update is called once per frame
        void Update()
        {
            if (combat.IsAttacking() == false)
                move.Move();
            if (Input.GetKeyDown(KeyCode.Mouse0))
                combat.Attack();
        }
    }
}