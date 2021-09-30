using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TrainRPG
{

    public class BanditBehaviour : MonoBehaviour
    {
        public float detectionRadius = 10;
        public float detectionAngle = 90;
        public float timeToStopPersuit = 2;
        public float timeCounterToPersuit = 0;

        private PlayerController m_Target;
        private NavMeshAgent m_NavMeshAgent;
        private Vector3 m_StartPos;
        private Animator m_Animator;


        private readonly int m_EnemyNearBase = Animator.StringToHash("toBase");
        private readonly int m_EnemyPersuit = Animator.StringToHash("onPersuite");

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_StartPos = transform.position;
            m_Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            Vector3 toBase = m_StartPos - transform.position;
            toBase.y = 0;
            m_Animator.SetBool(m_EnemyNearBase, toBase.magnitude <= 0.01f);
            var target = LookingForPlayer();

            if (!m_Target)
            {
                if (target != null)
                {
                    m_Target = target;
                }
            }
            else
            {
                m_NavMeshAgent.SetDestination(m_Target.transform.position);
                m_Animator.SetBool(m_EnemyPersuit, true);
                if (target == null)
                {
                    timeCounterToPersuit += Time.deltaTime;
                    if (timeCounterToPersuit >= timeToStopPersuit)
                    {
                        m_Target = null;
                        m_NavMeshAgent.isStopped = true;
                        m_Animator.SetBool(m_EnemyPersuit, false);
                        StartCoroutine(WaitOnPursuit());
                    }
                }
                else
                {
                    timeCounterToPersuit = 0;
                }
            }
        }

        IEnumerator WaitOnPursuit()
        {
            yield return new WaitForSeconds(2);
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.SetDestination(m_StartPos);
        }
        PlayerController LookingForPlayer()
        {
            if (PlayerController.Instance == null)
            {
                return null;
            }

            Vector3 enemyPos = transform.position;
            Vector3 toPlayer = PlayerController.Instance.transform.position - enemyPos;
            toPlayer.y = 0;

            if (toPlayer.magnitude <= detectionRadius)
            {
                if (Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    return PlayerController.Instance;
                }
            }
            else
            {
                Debug.Log("Where are you?");
            }
            return null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color c = new Color(0, 0, 0.7f, 0.4f);
            UnityEditor.Handles.color = c;
            Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadius);
        }
#endif



    }

}

