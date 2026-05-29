using UnityEngine;

/*
 * This class provides access to certain animation events in an Entity's animations
 */
public class AnimationListener : MonoBehaviour
{
    public delegate void AnimationPoint();
    public event AnimationPoint meleeAttackStart;
    public event AnimationPoint meleeAttackEnd;

    private void MeleeAttackStart() {
        meleeAttackStart?.Invoke();
    }

    private void MeleeAttackEnd() {
        meleeAttackEnd?.Invoke();
    }
}
