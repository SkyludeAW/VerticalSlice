using UnityEngine;

public class HealthPack : MonoBehaviour, ICollectable {
    [field: SerializeField] public Collider2D Collider { get; private set; }

    public void Collect(Entity target) {
        target.TakeDamage(-20f, Vector2.zero, null, false); // Heal the player by 20 health points (negative damage)
    }

    public void OnTriggerEnter2D(Collider2D other) {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null && entity.IsA(EntityAttribute.Player)) {
            Collect(entity);
            Destroy(gameObject); // Destroy the health pack after collection
        }
    }
}
