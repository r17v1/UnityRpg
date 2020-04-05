using UnityEngine;
using RPG.Helper;

namespace RPG.movement
{
    public class PlayerMovement : MonoBehaviour
    {
        // Start is called before the first frame update

        CharacterController characterController;
        [SerializeField]private float walkSpeed = 4.0f;
        [SerializeField]private float sprintSpeed = 7.0f;
        [SerializeField]private float jumpSpeed = 8.0f;
        [SerializeField]private float gravity = 20.0f;
        [SerializeField]private float rotationSpeed = 8.0f;
        [SerializeField] public float dodgeTime = 0.25f;
        public float dodging = 0f;

        private Animator animator;
        private Vector3 moveDirection = Vector3.zero;
        private Camera mainCamera;

        void Start()
        {
            
            mainCamera = Camera.main;
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        public void Move()
        {
            

            var currentSpeed = GetCurrentSpeed();
            if (characterController.isGrounded)
            {
                moveDirection = new Vector3(0f, 0.0f, 0.0f);
                
                    Rotate();
                    moveDirection = new Vector3(0f, 0.0f, 1.0f);
                    moveDirection = transform.TransformDirection(moveDirection);
                    moveDirection *= currentSpeed;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GetComponent<Animator>().SetTrigger("jump");
                        moveDirection.y = jumpSpeed;
                    }

                

                
            }
            
            moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
            AnimateMovement();
        }

        public float GetCurrentSpeed()
        {
            var inputX = Input.GetAxis("Vertical");
            var inputY = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputX) < 0.01f && Mathf.Abs(inputY) < 0.01f)
                return 0;
            else if (Input.GetAxis("Sprint") > 0)
                return sprintSpeed;
            else return walkSpeed;
        }


        public void AnimateMovement()
        {
            var speed = GetCurrentSpeed();
            if (!characterController.isGrounded)
                animator.SetFloat("forwardSpeed", 0);
            else if (speed == 0)
                animator.SetFloat("forwardSpeed", 0);
            else if (speed == walkSpeed)
                animator.SetFloat("forwardSpeed", 0.3f);
            else animator.SetFloat("forwardSpeed", 1f);

        }
        
        public void Rotate()
        {
            var inputX = Input.GetAxis("Vertical");
            var inputY = Input.GetAxis("Horizontal");
            if (Mathf.Abs(inputX) < 0.01f && Mathf.Abs(inputY) < 0.01f) return;
            var angle = Mathf.Atan2(inputY, inputX); 
            angle = Mathf.Rad2Deg * angle;
            angle += mainCamera.transform.eulerAngles.y;

            var targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        public void LookAtEnemy(Transform enemy)
        {
            Debug.Log("lookat");
            if (enemy != null)
            {
                transform.rotation= Quaternion.Slerp(transform.rotation, Rotation.LookAtY(transform.position, enemy.position),rotationSpeed*Time.deltaTime);
            }
        }

       
    }
}