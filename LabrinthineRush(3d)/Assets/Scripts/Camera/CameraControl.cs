using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.camera
{
    public class CameraControl : MonoBehaviour
    {
        // Start is called before the first frame update

        public Transform player;

        private float distance = 6f, minDistance = 4f, maxDistance = 10f;
        [SerializeField] private float sensitivityX = 4.0f;
        [SerializeField] private float sensitivityY = 1.0f;
        private float currentX = 0f;
        private float currentY = 0f;
        private float yAngleMin = 0;
        private float yAngleMax = 50;

        void Update()
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            distance += 2f * Input.GetAxis("Mouse ScrollWheel");

            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            currentY = Mathf.Clamp(currentY, yAngleMin, yAngleMax);
        }


        private void LateUpdate()
        {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = player.position + rotation * dir;
            transform.LookAt(player);
        }
    }
}