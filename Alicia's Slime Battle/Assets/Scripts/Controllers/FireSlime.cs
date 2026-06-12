using UnityEngine;
using System.Collections;

public class FireSlime : BasicSlime {
    protected override void AttackHit(Entity target) {
        if (!target.IsA(EntityAttribute.Fire)) {
            target.TakeDamage(EssentialAttributes.attackDamage, EssentialAttributes.attackKnockback * aimDirection, this);
            target.ApplyStatusEffect(new BurningEffect(5f, 2f)); 
        }
    }

    public override void Die() {
        GameManager.Instance.IncreaseScore(2);
        Destroy(this.gameObject, 0.1f);
    }
}
