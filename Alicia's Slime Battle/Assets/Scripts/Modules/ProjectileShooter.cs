using UnityEngine;
using UnityEngine.Pool;

/*
 * Class that implements the ability to shoot projectiles
 */

public class ProjectileShooter : MonoBehaviour {
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private Transform startingPosition;

    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxCapacity;
    private IObjectPool<Projectile> projectilePool;

    private void Awake() {
        // Initialization of the projectile pool
        projectilePool = new ObjectPool<Projectile>(() => {
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.Pool = projectilePool;
            projectile.origin = GetComponent<Entity>();
            projectile.pierceCount = 0;
            projectile.BeginPurgeCountdown(30f);
            return projectile;
        }, (Projectile projectile) => {
            projectile.trailRenderer.Clear();
            projectile.gameObject.SetActive(true);
        }, (Projectile projectile) => {
            projectile.gameObject.SetActive(false);
        }, (Projectile projectile) => {
            Destroy(projectile.gameObject);
        }, true, defaultCapacity, maxCapacity);
    }

    // Pulls a projectile from the pool and fires it by initializing its attributes
    public void Shoot(float damage, float knockback, int maxPierceCount, Vector2 direction, float speed) {
        Projectile projectile = projectilePool.Get();
        projectile.damage = damage;
        projectile.knockback = knockback;
        projectile.maxPierceCount = maxPierceCount;
        projectile.pierceCount = maxPierceCount;
        projectile.direction = direction;
        
        projectile.transform.position = startingPosition.position;
        projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(projectile.direction.y, projectile.direction.x) * Mathf.Rad2Deg - 90f);
        projectile.rb.AddForce(speed * direction);
    }
}
