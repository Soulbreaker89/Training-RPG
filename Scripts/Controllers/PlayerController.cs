using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainRPG
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10;
        public float rotationSpeed = 8;
        private Camera m_Cam; 
        private CharacterController m_ChController;
        private PlayerInput m_PlayerInput;

        private void Awake()
        {
            m_ChController = GetComponent<CharacterController>();
            m_Cam = Camera.main;
            m_PlayerInput = GetComponent<PlayerInput>();
        }

        void FixedUpdate()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput;            
            Quaternion camRot = m_Cam.transform.rotation;
            Vector3 targetDir = camRot * moveInput;
            targetDir.y = 0;


            m_ChController.Move(targetDir.normalized * speed * Time.fixedDeltaTime);
            m_ChController.transform.rotation = Quaternion.Euler(0, camRot.eulerAngles.y, 0);


        }
    }

}
