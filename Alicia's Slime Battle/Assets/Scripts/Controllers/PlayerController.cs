using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/*
 * This class controls the player
 */
public class PlayerController : Entity
{
    private Vector2 movement;
    [SerializeField] private HealthBar healthBar; 


    [SerializeField] private AnimationListener animationListener;
    private bool canChangeAnimationState;

    // Variables for handling dash
    private bool canDash;
    private DashController dashController;
    [SerializeField] private float dashCooldown;
    private Vector2 dashDirection;

    // Variables for handling attacks
    private ProjectileShooter projectileShooter;
    private bool canAttack;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int rangedAttackPierceCount;
    [SerializeField] private float rangedAttackProjectileSpeed;

    [SerializeField] private MeleeAttack meleeAttack;

    public delegate void playerDie();
    public event playerDie PlayerDied;

    // Vignette effect on low health
    [SerializeField] Volume postProcessingVolume;
    Vignette vignette;

    private void Awake() {
        // 反正就是一堆玩意的初始化
        rb = GetComponent<Rigidbody2D>();
        projectileShooter = GetComponent<ProjectileShooter>();
        dashController = GetComponent<DashController>();
        EssentialAttributes.attributes.Add(EntityAttribute.Player);

        animationListener.meleeAttackStart += MeleeAttackStart;
        animationListener.meleeAttackEnd += MeleeAttackEnd;
        meleeAttack.TargetHit += AttackHit;

        dashController.dashComplete += () => {
            StartCoroutine(StartDashCooldown());
        };

        movement = Vector2.zero;
        canDash = true;
        aimDirection = Vector2.zero;
        dashDirection = Vector2.zero;
        canAttack = true;
        canChangeAnimationState = true;

        health = EssentialAttributes.maxHealth;
        healthBar?.SetMaxHealth((int)health); // Initialize the health bar

        // Initialize vignette effect
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet<Vignette>(out vignette)) {
            vignette.intensity.value = 0f; // Initial vignette intensity
        }
    }

    // Update handles the input and visual parts of player actions
    protected override void Update() {
        base.Update();

        // Player movements input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        // Gets the direction the player is aiming at
        // 这行代码的功能就是找到鼠标与玩家之间的屏幕上位置差，用一个 Vector3 来表示，对找到玩家瞄准的方向有重要作用
        Vector3 pointerDirection = CameraLocator.Instance.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (pointerDirection != Vector3.zero) {
            aimDirection = pointerDirection;
        }

        // Player changes sprite based on direction facing
        // 玩家会根据鼠标指针朝向更换当前的 sprite
        if (movement != Vector2.zero) {
            if (canChangeAnimationState) {
                ChangeAnimationState(AnimationState.AnimationStates.PLAYER_MOVING);
                if (movement.x < 0)
                {
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    //spriteRenderer.flipX = false;
                }
                else if (movement.x > 0)
                {
                    transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                    //spriteRenderer.flipX = true;
                }
            }
        } else {
            if (canChangeAnimationState)
                ChangeAnimationState(AnimationState.AnimationStates.PLAYER_IDLE);
        }

        // Player dash movement input
        // 目前的 dash 按键被设置为了空格键，想改的话建议之后直换成用 Unity 最新的 Input Manager
        if (canDash && Input.GetKeyDown(KeyCode.Space) && movement != Vector2.zero) {
            canDash = false;
            dashController.StartDash(movement.normalized, true);
        }

        // Player attack
        if (canAttack && Input.GetKeyDown(KeyCode.Mouse1)) {
            projectileShooter.Shoot(EssentialAttributes.attackDamage * 0.75f, EssentialAttributes.attackKnockback * 0.5f, rangedAttackPierceCount, aimDirection.normalized, rangedAttackProjectileSpeed);
            canAttack = false;
            StartCoroutine(StartAttackCooldown());
        } else if (canAttack && Input.GetKeyDown(KeyCode.Mouse0)) {
            StartCoroutine(StartMeleeAttack());
            canAttack = false;
        }  
    }

    // Fixedupdate handles the physics parts of player actions
    private void FixedUpdate() {
        // Player movements handling
        // 移动方面，我选择了给 Rigidbody2D 加 impulse force 的方式；使用这种移动方式而非直接在 velocity 或者 position 上动手脚的好处在于可以做出更丝滑的移动与其他 entity 的碰撞效果
        rb.AddForce(movement * EssentialAttributes.speed * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    // TO-DO
    public override void Die() {
        Debug.Log("Player Died!");
        healthBar?.SetHealth(0);
        PlayerDied?.Invoke();
    }

    #region MeleeAttack
    // Player performs a melee attack in front of her, melee attack box collider is activated and deactivated by animation events
    private IEnumerator StartMeleeAttack() {
        canChangeAnimationState = false;
        ChangeAnimationState(AnimationState.AnimationStates.PLAYER_MELEE_ATTACK);

        yield return new WaitForEndOfFrame();
        transform.localScale = new Vector3((aimDirection.x < 0) ? 1 : (aimDirection.x > 0) ? -1 : transform.localScale.x, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ChangeAnimationState(AnimationState.AnimationStates.PLAYER_IDLE);
        canChangeAnimationState = true;

        StartCoroutine(StartAttackCooldown());
    }

    private void MeleeAttackStart() {
        rb.AddForce(aimDirection * EssentialAttributes.speed);
        meleeAttack.attackCollider.enabled = true;
        meleeAttack.hitEffect.Play();
    }

    private void MeleeAttackEnd() {
        meleeAttack.attackCollider.enabled = false;
    }

    private void AttackHit(Entity target) {
        target.TakeDamage(EssentialAttributes.attackDamage, EssentialAttributes.attackKnockback * aimDirection.normalized, this);
    }
    #endregion
    //code that updates the healthbar
    public override void TakeDamage(float damage, Vector2 force = default, Entity origin = null, bool causeInvincibility = true) {
        base.TakeDamage(damage, force, origin, causeInvincibility);
        healthBar?.SetHealth((int)health); // Update health bar when taking damage

        vignette.intensity.value = (1 - health / EssentialAttributes.maxHealth) * 0.3f; // Increase vignette intensity as health decreases
    }

    public override void RemoveHealth(float damage, Vector2 force = default, Entity origin = null) {
        base.RemoveHealth(damage, force, origin);
        healthBar?.SetHealth((int)health); // Update health bar when removing health 
    }

    // Timer for attack cooldown
    private IEnumerator StartAttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    
    // Timer for dash cooldown
    private IEnumerator StartDashCooldown() {
        // 冲累了，歇会
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

