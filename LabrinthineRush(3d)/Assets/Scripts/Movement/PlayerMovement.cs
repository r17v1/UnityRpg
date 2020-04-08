using UnityEngine;
using RPG.Helper;

namespace RPG.movement
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

       
        public float deltaTime;


        public float attackCooldownTime = 0.3f;
        float attackCooldown = 0f;


        float dodgeCooldownTime = 0.1f;
        float dodgeCooldown=0;
  
        


        void Start()
        {
            
            camera = Camera.main.transform;
            controller = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
        }
        public void Tick()
        {
            if (controller.isGrounded) fallVelocity = 0;
            if (twoHand) currentAttacks = thAttacks;
            else currentAttacks = ohAttacks;
            canMove = anim.GetBool("canMove");
            Gravity();

            if (canMove == false ||crossFading>0)
            {
                crossFading -= deltaTime;
                if (attack)
                    anim.SetBool("nextAttackChain", true);
                return;
            }
            anim.SetBool("nextAttackChain", false);

            if(dodgeCooldown>0)
            {
                dodgeCooldown -= Time.deltaTime;
                return;
            }

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


      void Animate()
      {

            anim.SetBool("twoHand", twoHand);

            if (lockOn)
            {

                if (run)
                {
                    smoothHorizontal = FloatLerp(smoothHorizontal, horizontal, 6 * Time.deltaTime);
                    smoothVertical = FloatLerp(smoothVertical, vertical, 6 * Time.deltaTime);
                }
                else
                {
                    smoothHorizontal = FloatLerp(smoothHorizontal, horizontal / 2f, 4 * deltaTime);
                    smoothVertical = FloatLerp(smoothVertical, vertical / 2f, 4 * deltaTime);
                }

                anim.SetFloat("rollV", MyCeil(vertical, 0.3f));
                anim.SetFloat("rollH", MyCeil(horizontal, 0.3f));
                anim.SetFloat("vertical", smoothVertical);
                anim.SetFloat("horizontal", smoothHorizontal);
                anim.SetBool("lockOn", true);

            }
            else
            {

                anim.SetFloat("vertical", moveAmount);
                anim.SetFloat("rollV", MyCeil(moveAmount, 0.3f));
                anim.SetFloat("rollH", 0f);
                anim.SetFloat("horizontal", 0f);
                anim.SetBool("lockOn", false);
            }
            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
                

            }
            else if (attack) {
               
               
                anim.CrossFade(currentAttacks[0], crossFadeTime);
                //anim.Play(currentAttacks[0]);
                crossFading = crossFadeTime;
                attackCooldown = attackCooldownTime; 
            }
            if (dodge)
            {
               
                anim.CrossFade("dodge", crossFadeTime);
                //anim.Play("dodge");
                crossFading = crossFadeTime;
                dodgeCooldown = dodgeCooldownTime;
            }
        }



        float MyCeil(float f, float tolerance)
        {
            if (f > 0)
            {
                if (f > tolerance) return 1;
                else return 0;
            }
            else
            {
                if (f < -1f * tolerance) return -1;
                else return 0;
            }
        }

        float FloatLerp(float initialValue, float finalValue, float speed)
        {
            if (initialValue > finalValue) speed = -speed;

            float r = initialValue;
            r += speed;
            return Mathf.Clamp(r, Mathf.Min(initialValue, finalValue), Mathf.Max(initialValue, finalValue));
        }


        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision);
        }


    }
}