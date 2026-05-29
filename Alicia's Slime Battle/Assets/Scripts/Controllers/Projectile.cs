using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/*
 * This class represents any projectile in the game
 */

public class Projectile : MonoBehaviour
{
    [field:SerializeField] public Rigidbody2D rb { get; private set; }
    [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }
    [field: SerializeField] public TrailRenderer trailRenderer { get; private set; }

    // Base damage of the projectile
    [SerializeField] public float damage;

    // Base knockback force of the projectile
    [SerializeField] public float knockback;

    // How many targets the projectile is able to pierce
    [SerializeField] public int maxPierceCount;
    public int pierceCount;

    // Normalized direction and speed the projectile travels in, used to calculate knockback or other stuffs
    [SerializeField] public Vector2 direction;
    [SerializeField] public float speed;

    // The source entity of this projectile
    [SerializeField] public Entity origin;

    // The reference to the parent entity's object pool that has a public setter and private getter
    private IObjectPool<Projectile> pool;
    public IObjectPool<Projectile> Pool {
        set => pool = value;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Hitting a wall eliminates this projectile right away
        if (collision.gameObject.CompareTag("Wall")) {
            pool.Release(this);
            return;
        }

        // Hitting an entity that is not invincible deals damage to them and decreases the pierce count by 1; the projectile is eliminated when pierce count turns 0
        Entity hitEntity = collision.gameObject.GetComponent<Entity>();
        if (hitEntity != null && hitEntity != origin && !hitEntity.IsInvincible && pierceCount > 0) {
            hitEntity.TakeDamage(damage, direction * knockback, origin);
            if (--pierceCount <= 0) 
                pool.Release(this);
        }
    }

    public void BeginPurgeCountdown(float duration) {
        StartCoroutine(PurgeCountdown(duration));
    }
    IEnumerator PurgeCountdown(float duration) {
        yield return new WaitForSeconds(duration);
        pool.Release(this);
    }
}
