using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;

/*
 * This is attached to the melee attack game objects that has an IsTrigger attack collider
 */
public class MeleeAttack : MonoBehaviour {
    // 近战攻击的 collider; this is typically not enabled initially
    [SerializeField] public Collider2D attackCollider;
    [SerializeField] public VisualEffect hitEffect;

    [SerializeField] public Animator animator;

    public delegate void hit(Entity target);
    public event hit TargetHit;

    private void OnTriggerEnter2D(Collider2D collision) {
        // Checks if the collided collider's gameObject is an Entity
        Entity target = collision.GetComponent<Entity>();
        if (target != null) {
            TargetHit?.Invoke(target);
        }
    }
}
