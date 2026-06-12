using UnityEngine;

public interface ICollectable {
    public Collider2D Collider { get; }
    void Collect(Entity target);
}
