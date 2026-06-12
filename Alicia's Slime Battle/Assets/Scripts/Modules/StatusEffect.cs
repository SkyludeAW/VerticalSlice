using System;
using UnityEngine;

public abstract class StatusEffect {
    protected float appliedTime;
    public float? Duration;

    public StatusEffect(float? duration) {
        Duration = duration;
    }

    public virtual void OnApply(Entity entity) => appliedTime = Time.time;

    public virtual void Update(Entity entity) { }

    public virtual void OnRemove(Entity entity) { }

    public virtual Predicate<Entity> GetRemovalCondition() {
        return entity => (Duration.HasValue && Time.time >= appliedTime + Duration);
    }
}
