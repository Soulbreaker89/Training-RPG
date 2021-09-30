using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainRPG
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance
        {
            get
            {
                return s_Instance;
            }
        }
        public float forwardSpeed;
        public float rotationSpeed = 8;
        public float m_MaxRotationSpeed = 1200;
        public float m_MinRotationSpeed = 800;
        public float gravity = 10;

        private static PlayerController s_Instance;
        private Quaternion m_TargetDirection;
        private CameraController m_CamController; 
        private CharacterController m_ChController;
        private PlayerInput m_PlayerInput;
        private Animator m_PlayerAnimator;
        private float MaxForwardSpeed = 8;
        private float m_VerticalSpeed;
        private float DesiredForwardSpeed;
        private readonly int m_HashForwardSpeed = Animator.StringToHash("ForwardSpeed");
        private float currentAcceleration = 500;

        const float acceleration = 20;
        const float decceleration = 1000;


        private void Awake()
        {
            m_ChController = GetComponent<CharacterController>();
            m_CamController = Camera.main.GetComponent<CameraController>();
            m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerAnimator = GetComponent<Animator>();
            s_Instance = this;
        }

        void FixedUpdate()
        {
            ComputeForwardMotions();
            ComputeVericalMotions();
            ComputeRotation();

            if (m_PlayerInput.isMoving)
            {
                float rotationSpeed = Mathf.Lerp(m_MaxRotationSpeed, m_MinRotationSpeed, forwardSpeed / DesiredForwardSpeed);
                m_TargetDirection = Quaternion.RotateTowards(
                    transform.rotation,
                    m_TargetDirection,
                    rotationSpeed * Time.fixedDeltaTime);
                transform.rotation = m_TargetDirection;
            }
                 
        }

        private void OnAnimatorMove()
        {
            Vector3 movement = m_PlayerAnimator.deltaPosition;
            movement += Vector3.up * m_VerticalSpeed * Time.fixedDeltaTime;
            m_ChController.Move(movement);
        }

        void ComputeVericalMotions()
        {
            m_VerticalSpeed = -gravity;
        }
        void ComputeForwardMotions()
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
            Vector3 cameraDir = Quaternion.Euler(0, m_CamController.playerCam.m_XAxis.Value, 0) * Vector3.forward;
            Quaternion targetRotation;

            if (Mathf.Approximately(Vector3.Dot(moveInput, Vector3.forward), -1))
            {
                targetRotation = Quaternion.LookRotation(-cameraDir);
            }
            else
            {
                Quaternion movementRotation = Quaternion.FromToRotation(Vector3.forward, moveInput);
                targetRotation = Quaternion.LookRotation(movementRotation * cameraDir);
            }
            m_TargetDirection = targetRotation;
        }
    }

}
