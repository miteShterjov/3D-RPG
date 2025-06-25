using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float gizmoRadius = 0.4f;
        [SerializeField] Color gizmoColor;

        // Returns the next index in the patrol path, wrapping around to 0 if at the end
        // This is used to cycle through next waypoints in the patrol path
        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        // Returns the current waypoint in the patrol path
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        // Draws gizmos in the editor to visualize the patrol path
        // It draws spheres at each waypoint and lines connecting them
        // This helps in debugging and understanding the patrol route
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), gizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

    }

}