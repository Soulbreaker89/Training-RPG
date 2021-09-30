using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrainRPG
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector3 m_Movement;

        public Vector3 MoveInput
        {
            get
            {
                return m_Movement; 
            }
        }
        private void Update()
        {
            m_Movement.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        public bool isMoving
        {
            get
                {
                    return !Mathf.Approximately(MoveInput.magnitude, 0);
                }
        }
    }

}
