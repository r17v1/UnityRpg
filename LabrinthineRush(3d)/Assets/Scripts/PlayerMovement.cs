using UnityEngine;
using RPG.Helper;
using RPG.Combat;
namespace RPG.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        // Start is called before the first frame update

        public Vector3 moveDirection;

        public float vertical;
        public float horizontal;
        public float moveAmount;
        

        float smoothHorizontal = 0f;
        float smoothVertical = 0f;
        
        public float gravity=9.8f;
        public float walkSpeed=4f;
        public float runSpeed=7f;
        public float rotationSmoothSpeed = 5f;
        public float crossFadeTime = 0.1f;

        float crossFading = 0;

        float fallVelocity;


        float speed;

        public bool canMove;
        public bool lockOn;
        public bool twoHand;
        public bool dodge;
        public bool run;
        public bool attack;


        public string[] currentAttacks;
        public string[] ohAttacks;
        public string[] thAttacks;




        public Animator anim;
        public Transform camera;
        public CharacterController controller;
        public Stats stats;
       
        public float deltaTime;
        public float sprintStamina = 10f;

        CombatController combat;


        void Start()
        {
            stats = GetComponent<Stats>();
            camera = Camera.main.transform;
            controller = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            combat = GetComponent<CombatController>();
        }
        public void Tick()
        {
            if (controller.isGrounded) fallVelocity = 0;
           
            canMove = anim.GetBool("canMove");
            UpdateCombat();
            Gravity();

            if (run)
            {
                stats.reduceStamina(sprintStamina * Time.deltaTime);
            }         

            if(canMove ==false)
                return;

            if(combat.dodgeCooldown>0)
                return;
  

            if (controller.isGrounded == false) return;
            Rotate();
            GetSpeed();
            Move();
            Animate();
        }
        
      

        void GetSpeed()
        {
            speed = run ? runSpeed : walkSpeed;
        }

        void Gravity()
        {
            fallVelocity += gravity * deltaTime;
            float fallDistance = fallVelocity* deltaTime;
            controller.Move(new Vector3(0, -fallDistance, 0));
        }


        public void Move()
        {
            moveDirection *= speed;
            controller.Move(moveDirection * deltaTime);
        }
        void Rotate()
        {
            if (lockOn)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,camera.rotation.y,0), deltaTime * rotationSmoothSpeed);
                return;
            }

            if (Mathf.Abs(vertical) < 0.01f && Mathf.Abs(horizontal) < 0.01f && !lockOn) return;
            var angle = Mathf.Atan2(horizontal, vertical);
            angle = Mathf.Rad2Deg * angle;
            angle += Camera.main.transform.eulerAngles.y;

            var targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, deltaTime * rotationSmoothSpeed);
        }


        void UpdateCombat()
        {
            combat.updateVariables(deltaTime, twoHand, ohAttacks, thAttacks, 0, canMove);
            
            if(lockOn)
            {
                combat.setDodgeValirable(MyFloat.Round01Tollerence(vertical, 0.3f), MyFloat.Round01Tollerence(horizontal, 0.3f));
            }
            else
            {
                combat.setDodgeValirable(MyFloat.Round01Tollerence(moveAmount, 0.3f),0f);
            }
            
            
            combat.Tick();
            if(attack)
                combat.Attack();
            if (dodge)
                combat.Dodge();


        }
      void Animate()
      {

            anim.SetBool("twoHand", twoHand);

            if (lockOn)
            {

                if (run)
                {
                    smoothHorizontal = MyFloat.FloatLerp(smoothHorizontal, horizontal, 6 * Time.deltaTime);
                    smoothVertical = MyFloat.FloatLerp(smoothVertical, vertical, 6 * Time.deltaTime);
                }
                else
                {
                    smoothHorizontal = MyFloat.FloatLerp(smoothHorizontal, horizontal / 2f, 4 * deltaTime);
                    smoothVertical = MyFloat.FloatLerp(smoothVertical, vertical / 2f, 4 * deltaTime);
                }

                anim.SetFloat("vertical", smoothVertical);
                anim.SetFloat("horizontal", smoothHorizontal);
                anim.SetBool("lockOn", true);

            }
            else
            {

                anim.SetFloat("vertical", moveAmount);
                anim.SetFloat("horizontal", 0f);
                anim.SetBool("lockOn", false);
            }
        }



        


        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision);
        }


    }
}