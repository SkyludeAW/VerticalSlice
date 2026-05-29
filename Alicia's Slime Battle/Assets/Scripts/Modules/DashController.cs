using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

/*
 * This class gives the entity dashing capabilities
 */

public class DashController : MonoBehaviour {
    // Reference to the host entity dashing
    [SerializeField] private Entity host;

    // Basic dash attributes
    [SerializeField] private float dashPower;
    [SerializeField, Range(0.001f, 1f)] private float dashDuration;
    [SerializeField] private AnimationCurve dashCurve;

    // Layers for handling collision in the Physics2D collision matrix
    private int entityLayer;
    private int ghostLayer;

    // Object pool and other variables for the afterimage effect
    // 有残影就是帅！
    private bool isDashing;
    private IObjectPool<Afterimage> afterimagePool;
    [SerializeField] private Afterimage afterimagePrefab;
    [SerializeField] private float afterimageDuration;
    [SerializeField, Range(0.001f, 0.25f)] private float afterimageInterval;
    [SerializeField] private Color afterimageColor;
    [SerializeField] private int defaultAfterimageCapacity;
    [SerializeField] private int maxAfterimageCapacity;
    private float nextTimeToSpawnAfterimage;

    // Event system for notifying dash completed
    public delegate void DashComplete();
    public event DashComplete dashComplete;

    // Blur effect
    [SerializeField] private Material blurMaterial;

    private void Awake() {
        entityLayer = LayerMask.NameToLayer("Entity");
        ghostLayer = LayerMask.NameToLayer("Ghost");

        afterimagePool = new ObjectPool<Afterimage>(() => {
            Afterimage afterimage = Instantiate(afterimagePrefab);
            afterimage.Pool = afterimagePool;
            return afterimage;
        }, (Afterimage afterimage) => {
            afterimage.gameObject.SetActive(true);
            afterimage.transform.position = host.spriteRenderer.transform.position;
            afterimage.Duration = afterimageDuration;
            afterimage.SetSprite(host.spriteRenderer.sprite, afterimageColor, host.transform.localScale);
        }, (Afterimage afterimage) => {
            afterimage.gameObject.SetActive(false);
        }, (Afterimage afterimage) => {
            Destroy(afterimage.gameObject);
        }, true, defaultAfterimageCapacity, maxAfterimageCapacity);
        nextTimeToSpawnAfterimage = 0;
    }

    public void StartDash(Vector2 dashDirection, bool invincibleDuringDash = false) {
        StartCoroutine(Dash(dashDirection, invincibleDuringDash));
    }

    // Host gains invincibility frames and ignores collisions with other entities during the dash 
    // 冲刺时有无敌帧，并无视与其他 entity 的碰撞
    private IEnumerator Dash(Vector2 dashDirection, bool invincibleDuringDash = false) {
        // 冲刺！
        float elapsed = 0f;
        nextTimeToSpawnAfterimage = 0f;

        if (invincibleDuringDash)
            host.SetInvincibilityEndTime(Mathf.Max(host.InvincibilityEndTime, Time.time + dashDuration));

            // 在 project setting 里有一个 Physics2D 的碰撞矩阵，其中 ghost layer 无法与正常的 entity 产生碰撞；在冲刺过程中原本的 layer，也就是 entity layer，会被暂时调到这个 layer 里，以达成冲刺过程中可以越过其他 mob 的效果
            gameObject.layer = ghostLayer;

        // Instantaneous velocities at specific time points during the dash may be altered by the dash curve
        // 这里的 dash velocity over time 可以用那个叫 [SerializeField] AnimationCurve dashCurve 的变量在 inspector 界面手动调
        while (elapsed < dashDuration) {

            // Normalizes elapsed time since the dash began
            float t = elapsed / dashDuration;

            // Dash velocity depends on current point on dash curve
            host.rb.linearVelocity = dashDirection * dashPower * host.EssentialAttributes.speed * dashCurve.Evaluate(t);
            elapsed += Time.deltaTime;

            if (elapsed >= nextTimeToSpawnAfterimage) {
                Afterimage afterimage = afterimagePool.Get();
                if (afterimage != null) {
                    afterimage.StartFade();
                    nextTimeToSpawnAfterimage = elapsed + afterimageInterval;
                }
            }

            // Blur
            blurMaterial.SetFloat("_BlurStrength", dashCurve.Evaluate(t) * 0.05f); // Adjust the multiplier as needed for desired blur intensity

            yield return null;
        }

        // 冲刺结束后不再无敌，并开始冲刺的 cooldown timer
        gameObject.layer = entityLayer;

        // Remove blur
        blurMaterial.SetFloat("_BlurStrength", 0f);

        Afterimage lastAfterimage = afterimagePool.Get();
        if (lastAfterimage != null)
            lastAfterimage.StartFade();

        dashComplete?.Invoke();
    }


}
