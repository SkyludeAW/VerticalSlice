using UnityEngine;

/*
 * Positions the melee attack relative to the parent Entity's transform based on the direction they are aiming at; works similarly to how CameraHandler class handles the CameraTarget game object
 */
public class MeleeAttackPositioner : MonoBehaviour {
    [SerializeField] private Entity parent;
    [SerializeField] private Collider2D attackCollider;

    [SerializeField] public float offsetDistance;
    [SerializeField] public float size;

    private void Update() {
        transform.localScale = Vector3.one * size;
        Vector2 direction = parent.AimDirection.normalized;
        transform.position = (Vector2) parent.transform.position + direction * offsetDistance;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
    }
}
