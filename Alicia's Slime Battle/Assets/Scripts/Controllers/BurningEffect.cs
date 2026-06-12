using UnityEngine;

public class BurningEffect : StatusEffect {
    private readonly GameObject burnEffectPrefab = Resources.Load<GameObject>("Prefabs/Fire VFX");

    private GameObject burnEffectInstance;

    public float TickInterval = 0.1f;
    public float DamagePerTick = 1f;

    private float lastTickTime;

    public BurningEffect(float duration, float tickInterval, float damagePerTick) : base(duration) {
        TickInterval = tickInterval;
        DamagePerTick = damagePerTick;
    }

    public BurningEffect(float duration, float damagePerSecond) : base(duration) {
        TickInterval = 0.1f; // Default tick interval
        DamagePerTick = damagePerSecond * TickInterval; // Calculate damage per tick based on DPS
    }

    public override void OnApply(Entity entity) {
        base.OnApply(entity);

        burnEffectInstance = MonoBehaviour.Instantiate(burnEffectPrefab, entity.transform);
        burnEffectInstance.transform.localPosition = Vector3.zero;
    }

    public override void Update(Entity entity) {
        base.Update(entity);

        if (Time.time >= lastTickTime + TickInterval) {
            entity.RemoveHealth(DamagePerTick);
            lastTickTime = Time.time;
        }

    }

    public override void OnRemove(Entity entity) {
        base.OnRemove(entity);

        if (burnEffectInstance != null) 
            MonoBehaviour.Destroy(burnEffectInstance);
    }
}
