using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainRPG
{
    public class PlayerController : MonoBehaviour
    {
        const float acceleration = 20;
        const float decceleration = 335;
        private float currentAcceleration;

        public float forwardSpeed;
        public float rotationSpeed = 8;

        private Quaternion m_TargetDirection;
        private CameraController m_CamController; 
        private CharacterController m_ChController;
        private PlayerInput m_PlayerInput;
        private Animator m_PlayerAnimator;
        private float MaxForwardSpeed = 8;
        private float DesiredForwardSpeed;
        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");

        private void Awake()
        {
            m_ChController = GetComponent<CharacterController>();
            m_CamController = GetComponent<CameraController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerAnimator = GetComponent<Animator>();            
        }

        void FixedUpdate()
        {
            ComputeMotions();
            ComputeRotation();

            if (m_PlayerInput.isMoving)
            {
                transform.rotation = m_TargetDirection;
            }
        }

        void ComputeMotions()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;
            DesiredForwardSpeed = moveInput.magnitude * MaxForwardSpeed;
            currentAcceleration = m_PlayerInput.isMoving ? acceleration : decceleration;
            forwardSpeed = Mathf.MoveTowards(forwardSpeed, DesiredForwardSpeed, Time.deltaTime * currentAcceleration);
            m_PlayerAnimator.SetFloat(m_HashForwardSpeed, forwardSpeed);
            
        }

        void ComputeRotation()
        {
            Vector3 moveInput = m_PlayerInput.MoveInput.normalized;                      
            Quaternion targetRotation = Quaternion.LookRotation(Quaternion.FromToRotation(Vector3.forward, moveInput) * Quaternion.Euler(0, m_CamController.freeLookCamera.m_XAxis.Value, 0) * Vector3.forward);

            m_TargetDirection = targetRotation;


        }
    }

}
