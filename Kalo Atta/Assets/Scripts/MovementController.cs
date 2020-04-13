using UnityEngine;
using RPG.Combat;
using RPG.Helper;
using RPG.Controller;
namespace RPG.Movement
{
    public class MovementController : MonoBehaviour
    {
        public float crossFadeTime;
        public float rotationSpeed=4f;
        public float walkSpeed=3f;
        public float sprintSpeed=20f;
        public float dodgeStamina = 20f;
        public float sprintStamina = 5f;
        public float gravity = 9.81f;
        float fallVelocity;
        float crossFading;
        float vertical;
        float horizontal;
        float smoothVertical=0;
        float smoothHorizontal=0;
        float deltaTime;
        Vector3 moveDirection;
        bool canMove;
        bool cancelAnimation;
        bool previousCancelAnimation;
        bool dodge;
        bool sprint;
        bool lockOn=false;
        Animator anim;
        Stats stats;
        CharacterController controller;

        float cooldownTime=0.1f;
        float cooldown = 0;

        private void Start()
        {
            anim = GetComponent<Animator>();
            stats = GetComponent<Stats>();
            controller = GetComponent<CharacterController>();
        }

        public void Tick(float vertical, float horizontal, bool sprint, bool dodge,float deltaTime)
        {
            if (controller.isGrounded) fallVelocity = 0;
            Gravity(); 
            this.vertical = vertical;
            this.horizontal = horizontal;
            this.sprint = sprint;  
            this.dodge = dodge;
            if (Camera.main.transform.GetComponent<CameraControl>().lockOn != null)
                lockOn = true;
            else lockOn = false;
            this.deltaTime = deltaTime;
            anim.SetBool("lockOn",this.lockOn);
            canMove = anim.GetBool("canMove");
            previousCancelAnimation = cancelAnimation;
            cancelAnimation = anim.GetBool("cancelAnimation");
            if(previousCancelAnimation!=cancelAnimation)
                cooldown = cooldownTime;
            if (cooldown > 0) cooldown -= Time.deltaTime;

            Move();
            Dodge();
        }

        void Move()
        {
            float targetVertical=0f, targetHorizontal=0f,speed;
            if(sprint)
            {
                if (stats.currentStamina > 0)
                {
                    if (vertical > 0) targetVertical = 1f;
                    else if (vertical < 0) targetVertical = -1f;
                    if (horizontal > 0) targetHorizontal = 1f;
                    else if (horizontal < 0) targetHorizontal = -1f;
                    speed = sprintSpeed;
                    stats.reduceStamina(sprintStamina*deltaTime);
                }
                else
                {
                    sprint = false;
                    if (vertical > 0) targetVertical = 0.6f;
                    else if (vertical < 0) targetVertical = -0.6f;
                    if (horizontal > 0) targetHorizontal = 0.6f;
                    else if (horizontal < 0) targetHorizontal = -0.6f;
                    speed = walkSpeed;
                    stats.reduceStamina(sprintStamina);
                }
            }
            else
            {
                if (vertical > 0) targetVertical = 0.6f;
                else if (vertical < 0) targetVertical = -0.6f;
                if (horizontal > 0) targetHorizontal = 0.6f;
                else if (horizontal < 0) targetHorizontal = -0.6f;
                speed = walkSpeed;
            }
            if(lockOn==false)
            {
                targetVertical = Mathf.Max(Mathf.Abs(targetVertical), Mathf.Abs(targetHorizontal));
                targetHorizontal = 0f;
                
            }

            moveDirection = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z) * vertical) + (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z) * horizontal).normalized;

            if (moveDirection.magnitude == 0 || (!canMove && !cancelAnimation) || (cancelAnimation && cooldown > 0))
            {
                targetVertical = targetHorizontal = 0f;
                speed = 0f;
                sprint = false;
            }
            else { 
                Rotate();
                if(cancelAnimation && cooldown<=0)
                    anim.CrossFade("empty_override", 0.1f);
            }

            smoothHorizontal = MyFloat.FloatLerp(smoothHorizontal, targetHorizontal, 4 * deltaTime);
            smoothVertical = MyFloat.FloatLerp(smoothVertical, targetVertical, 4 * deltaTime);

           
            anim.SetFloat("vertical", smoothVertical);
            anim.SetFloat("horizontal", smoothHorizontal);

            controller.Move(moveDirection * speed * deltaTime);
        }
        void Rotate()
        {
            if (lockOn)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, Camera.main.transform.rotation.y, 0), deltaTime * rotationSpeed);
                return;
            }

            if (Mathf.Abs(vertical) < 0.01f && Mathf.Abs(horizontal) < 0.01f && !lockOn) return;
            var angle = Mathf.Atan2(horizontal, vertical);
            angle = Mathf.Rad2Deg * angle;
            angle += Camera.main.transform.eulerAngles.y;

            var targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, deltaTime * rotationSpeed);
        }

        void Dodge()
        {
            if (anim.GetBool("dodge")) return;
            if (anim.GetBool("dodging")) return;
            if (stats.currentStamina <= 0) return;
            if (!dodge) return;
            if (canMove == false && cancelAnimation == false) return;
            float rollV, rollH;
            if (lockOn)
            {
                rollV = MyFloat.Round01Tollerence(vertical, 0.3f);
                rollH = MyFloat.Round01Tollerence(horizontal, 0.3f);
            }
            else
            {
                rollV = MyFloat.Round01Tollerence(Mathf.Max(Mathf.Abs(vertical),Mathf.Abs(horizontal)), 0.3f);
                rollH = 0f;
            }

            anim.SetFloat("rollV", rollV);
            anim.SetFloat("rollH", rollH);
            if (canMove == false) anim.Play("emptyOverride");
            anim.SetBool("dodge",true);
            stats.reduceStamina(dodgeStamina);
            crossFading = crossFadeTime;
        }

        void Gravity()
        {
            fallVelocity += gravity * deltaTime;
            float fallDistance = fallVelocity * deltaTime;
            controller.Move(new Vector3(0, -fallDistance, 0));
        }
    }
}