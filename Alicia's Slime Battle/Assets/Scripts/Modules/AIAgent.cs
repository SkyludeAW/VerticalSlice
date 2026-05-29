using Pathfinding;
using UnityEngine;

/*
 * An AI agent utilizing the A* Pathfinding Project package for navigation on an Entity
 */
public class AIAgent : MonoBehaviour
{
    [SerializeField] public AIPath path;

    [SerializeField] public float speed;
    [SerializeField] public Transform target;

    private void Start() {
        path = GetComponent<AIPath>();
        target = PlayerLocator.Instance.transform;
    }

    private void Update() {
        path.maxSpeed = speed;
        path.destination = target.position;
    }
}
