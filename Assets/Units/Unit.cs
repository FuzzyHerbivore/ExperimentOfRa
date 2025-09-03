using UnityEngine;
using UnityEngine.AI;

namespace Assets.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class Unit : MonoBehaviour
    {
        public Transform Target { get; private set; }

        NavMeshAgent navMeshAgent;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (!TryGetComponent(out NavMeshAgent agent)) return;

            NavMeshPath path = agent.path;

            if (path.corners.Length < 2) return; // Need 2 corners to draw line

            for (int i = 1; i < path.corners.Length; i++)
            {
                Vector3 lastPosition = path.corners[i - 1];
                Vector3 currentPosition = path.corners[i];

                Gizmos.DrawLine(lastPosition, currentPosition);
            }
        }
    }
}
