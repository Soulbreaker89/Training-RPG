using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainRPG
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10;
        public float rotationSpeed = 8;
        public Camera cam;     
        private Rigidbody m_RigidBody;

        private void Awake()
        {
            m_RigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");

            if (dir == Vector3.zero)
            {
                return;
            }

            Vector3 cameraDirection = cam.transform.rotation * dir;
            Vector3 targetDirection = new Vector3(cameraDirection.x, 0, cameraDirection.z);

            if(dir.z >= 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.1f);
            }

            m_RigidBody.MovePosition(m_RigidBody.position + targetDirection.normalized * speed * Time.fixedDeltaTime);
            
        }
    }

}
