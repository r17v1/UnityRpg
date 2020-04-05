using UnityEngine;

namespace RPG.camera
{
    public class CameraControl : MonoBehaviour
    {
        // Start is called before the first frame update

        public Transform player;
        [SerializeField] private float inputDistance = 6f, minDistance = 2f, maxDistance = 5f;
        [SerializeField] private float sensitivityX = 4.0f;
        [SerializeField] private float sensitivityY = 1.0f;
        private float currentX = 0f;
        private float currentY = 0f;
        [SerializeField] private float yAngleMin = 0;
        [SerializeField] private float yAngleMax = 50;
        public LayerMask collisionMask;
        Vector3 playerOffset;
        [SerializeField]float offset=2f;
       

        void Update()
        {
            playerOffset = new Vector3(player.position.x, player.position.y + offset, player.position.z);
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            if (currentX >= 360f) currentX -= 360f;
            else if (currentX <= -360f) currentX += 360f;

            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            inputDistance += 2f * Input.GetAxis("Mouse ScrollWheel");

            inputDistance = Mathf.Clamp(inputDistance, minDistance, maxDistance);
            currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);

        }


        private void LateUpdate()
        {
            float distance = inputDistance;
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = playerOffset + rotation * dir;
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


        float HitDetection()
        {

            RaycastHit hit;
            if (Physics.Raycast(playerOffset, transform.position - playerOffset, out hit, inputDistance, collisionMask))
            {
                if (hit.collider.transform.parent != null&& hit.collider.transform.parent.name=="Stage")
                {
                    
                    return Vector3.Distance(hit.point, playerOffset);
                }
            }
            return -1;
            //Debug.DrawRay(ray.origin,ray.direction*50,Color.yellow);
        }

    }
}
