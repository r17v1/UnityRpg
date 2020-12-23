using UnityEngine;
using RPG.Helper;

namespace RPG.Controller
{
    public class CameraControl : MonoBehaviour
    {
        // Start is called before the first frame update

        public Transform player;
        [SerializeField] private float initialDistance = 3f, distance;
        [SerializeField] private float sensitivityX = 4.0f;
        [SerializeField] private float sensitivityY = 1.0f;
        private float currentX = 0f;
        private float currentY = 0f;
        [SerializeField] private float yAngleMin = 0;
        [SerializeField] private float yAngleMax = 50;
        public LayerMask collisionMask;
        Vector3 playerOffset;
        [SerializeField] float offset = 2f;

        public Transform lockOn = null;
        bool follow = true;


        public void Init(Transform target)
        {
            player = target;
        }




        public void Tick()
        {


            GetMouseMovement();

            MoveCamera();

        }


        public void GetMouseMovement()
        {
            playerOffset = new Vector3(player.position.x, player.position.y + offset, player.position.z);
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            if (currentX >= 360f) currentX -= 360f;
            else if (currentX <= -360f) currentX += 360f;

            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            //inputDistance += 2f * Input.GetAxis("Mouse ScrollWheel");

            //inputDistance = Mathf.Clamp(inputDistance, minDistance, maxDistance);
            currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
        }


        public void MoveCamera()
        {
            distance = initialDistance;

            if (lockOn == null)
            {
                Vector3 dir = new Vector3(0, 0, -distance);
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                var targetPos = playerOffset + rotation * dir;
                //transform.position = targetPos;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 4);  //edit

                transform.LookAt(playerOffset);

                distance = HitDetection();
                if (distance != -1)
                {
                    distance -= 0.1f;
                    dir = new Vector3(0, 0, -distance);
                    rotation = Quaternion.Euler(currentY, currentX, 0);
                    transform.position = playerOffset + rotation * dir;
                    transform.LookAt(playerOffset);
                }
            }
            else
            {


                player.rotation = Rotation.LookAtY(player.position, lockOn.position);
                var target = lockOn.transform.position;
                Vector3 dir = target - playerOffset;
                dir.y = 0;
                dir.Normalize();
                var targetPos = playerOffset + dir * distance * -1f;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 4);
                transform.LookAt(playerOffset);



                distance = HitDetection();
                if (distance != -1)
                {
                    distance -= 0.1f;
                    dir = target - playerOffset;
                    dir.y = 0;
                    dir.Normalize();

                    transform.position = playerOffset + dir * distance * -1f;

                    transform.LookAt(playerOffset);

                }
            }
        }


        float HitDetection()
        {

            RaycastHit hit;
            if (Physics.Raycast(playerOffset, transform.position - playerOffset, out hit, distance, collisionMask))
            {
                if (hit.collider.transform.parent != null && hit.collider.transform.parent.name == "Stage")
                {

                    return Vector3.Distance(hit.point, playerOffset);
                }
            }
            
            return -1;
        }

    }
}
