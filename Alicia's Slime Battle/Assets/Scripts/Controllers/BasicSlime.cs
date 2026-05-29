using System.Collections;
using UnityEngine;

/*
 * Just a lil' harmless cute slime...
 */

public class BasicSlime : Entity
{
    [SerializeField] private HealthBar healthBar; 
    [SerializeField] private AnimationListener animationListener;

    [SerializeField] private MeleeAttack meleeAttack;
    [SerializeField] private float attackCooldown;
    private bool canAttack;
    [SerializeField] private float jumpStrength;

    [SerializeField] private AIAgent agent;
    [SerializeField] private float viewDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private bool drawGizmos;
    [SerializeField] private AudioSource audioSource;

    private void Awake() {
        // 反正就是一堆玩意的初始化
        rb = GetComponent<Rigidbody2D>();

        health = EssentialAttributes.maxHealth;
        healthBar?.SetMaxHealth((int)health); // Initialize the health bar

        animationListener.meleeAttackStart += AttackStart;
        animationListener.meleeAttackEnd += AttackEnd;
        canAttack = true;
        meleeAttack.TargetHit += AttackHit;
    }

    private void Update() {
        agent.path.maxAcceleration = EssentialAttributes.speed;

        float distanceToTarget = (agent.target.position - transform.position).magnitude;
        if (distanceToTarget <= viewDistance) { 
            if (canAttack)
                agent.path.canMove = true;
            if (distanceToTarget < attackDistance) {
                agent.path.canMove = false;
                if (canAttack) {
                    aimDirection = agent.target.position - transform.position;
                    aimDirection.Normalize();
                    StartCoroutine(Attack());
                    canAttack = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F) && canAttack) {
            aimDirection = CameraLocator.Instance.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            aimDirection.Normalize();
            StartCoroutine(Attack());
            canAttack = false;
        }
    }

    public override void Die() {
        GameManager.Instance.IncreaseScore(1);
        Destroy(this.gameObject);
    }

    // Performs a jump attack at aimDirection; jump distance is determined with jumpStrength
    private IEnumerator Attack() {
        // AttackStart() and AttackEnd() will be called as animation events during the BasicSlime_Jump animation
        ChangeAnimationState(AnimationState.AnimationStates.BASICSLIME_JUMP);

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ChangeAnimationState(AnimationState.AnimationStates.BASICSLIME_IDLE);

        StartCoroutine(StartAttackCooldown());
    }
    //update health when take damage
    public override void TakeDamage(float damage, Vector2 force = default, Entity origin = null, bool causeInvincibility = true)
    {
        audioSource.Play();
        base.TakeDamage(damage, force, origin, causeInvincibility);
        healthBar?.SetHealth((int)health); // Update health bar when taking damage
    }

    // This is called when the slime begins to jump, which enables its attack collider
    private void AttackStart() {
        rb.AddForce(aimDirection * jumpStrength);
        meleeAttack.attackCollider.enabled = true;
    }

    // Attack collider is disabled when the slime lands its jump
    private void AttackEnd() {
        meleeAttack.attackCollider.enabled = false;
    }

    // Triggers when the slime hits a melee attack
    private void AttackHit(Entity target) {
        //if (!target.IsA(EntityAttribute.Slime))
            target.TakeDamage(EssentialAttributes.attackDamage, EssentialAttributes.attackKnockback * aimDirection, this);
    }

    private IEnumerator StartAttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnDrawGizmos() {
        if (!drawGizmos) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere (transform.position, viewDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
