using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class represents any being/mob in the game, including the player
 */

public enum EntityAttribute {
    // Types of entity
    Player,
    Slime,
    Tentacle,
    Undead
}

public abstract class Entity : MonoBehaviour {
    // Contains maxHealth, speed, attackDamage, armor, attackKnockback, knockbackResistance
    [SerializeField] protected EntityData entityData;
    public EntityData EssentialAttributes => entityData;

    // 这里的 health 代表个体的当前生命值，而不是 entityData 里面的最大生命值
    [Tooltip("Entity's current health")]
    [SerializeField] protected float health;
    public float Health => health;

    // Direction this entity is aiming at
    protected Vector2 aimDirection;
    public Vector2 AimDirection => aimDirection;

    // The entity is invulnerable when isInvincible is true or invincibilityEndTime has not been reached (under invincibility frames)
    #region Invincibility Variables
    // If the entity is damageable
    [SerializeField] protected bool isInvincible;
    public bool IsInvincible => (isInvincible || Time.time < invincibilityEndTime);

    // How long the invincibility frames last
    [SerializeField] protected float invincibilityDuration;

    // When does invincibility end
    [SerializeField] protected float invincibilityEndTime;
    public float InvincibilityEndTime => invincibilityEndTime;
    #endregion

    #region Visuals
    // How long the turning-red visual lasts when taking damage
    [SerializeField] protected float hurtDuration = 0.25f;

    [field: SerializeField] public Rigidbody2D rb { get; protected set; }

    [field: SerializeField] public SpriteRenderer spriteRenderer { get; protected set; }
    [SerializeField] protected Animator animator;
    protected AnimationState.AnimationStates currentState;
    #endregion

    // Sets the isInvincible attribute
    public void SetInvincible(bool isInvincible) {
        this.isInvincible = isInvincible;
    }

    // Sets the invincibilityEndTime attribute
    public void SetInvincibilityEndTime(float invincibilityEndTime) {
        this.invincibilityEndTime = invincibilityEndTime;
    }

    // "force" represents potential knockback effects on this entity, "origin" represents the damage source, "causeInvincibility" marks if this damage will make the receiver enter invincibility frames (useful for differentiating continuous damages like poison)
    public virtual void TakeDamage(float damage, Vector2 force = default, Entity origin = null, bool causeInvincibility = true) {
        if (!IsInvincible) {
            Hurt(causeInvincibility);
            health -= damage;
            if (rb != null)
                rb.AddForce(force * (1f - EssentialAttributes.knockbackResistance));
            if (health <= 0f)
                Die();
        }
    }

    // What happens after the entity dies 💀; implemented explicitly in the child classes
    // 有自爆等功能的特殊亡语怪可以在这里被 implement
    public abstract void Die();

    // Represents what triggers when this entity took damage
    public virtual void Hurt(bool causeInvincibility = true) {
        StartCoroutine(EnterHurtFrames(hurtDuration));
        if (causeInvincibility)
            StartCoroutine(EnterInvincibilityFrames(invincibilityDuration));
    }

    // Visual effect of turning red and gradually fading back when the entity is hit
    // 受击后的红温效果
    protected virtual IEnumerator EnterHurtFrames(float hurtDuration) {
        float elapsed = 0f;

        while (elapsed < hurtDuration) {
            spriteRenderer.color = new Color(1f, Mathf.Min(elapsed / hurtDuration, 1f), Mathf.Min(elapsed / hurtDuration, 1f));
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    // Extending the invincibleEndTime by invincibilityDuration to gain invincibility for some time
    // 进入无敌帧；无敌持续时间为 invincibilityDuration
    // I don't even know why this thing is being implemented as a Coroutine lol
    protected virtual IEnumerator EnterInvincibilityFrames(float invincibilityDuration) {
        invincibilityEndTime = Mathf.Max(invincibilityEndTime, Time.time + invincibilityDuration);

        yield return null;
    }

    // Switches to the target animation state
    protected void ChangeAnimationState(AnimationState.AnimationStates targetState) {
        if (targetState != currentState) {
            currentState = targetState;
            animator.Play(AnimationState.StateToString(currentState));
        }
    }

    // Checks if this is some specific type of entity
    public bool IsA(EntityAttribute attribute) {
        return EssentialAttributes.attributes.Contains(attribute);
    }
}
