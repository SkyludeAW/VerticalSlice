using UnityEngine;

/*
 * I don't even know why this ugly thing exists at this point...
 */
public class AnimationState
{
    public enum AnimationStates {
        PLAYER_IDLE,
        PLAYER_MOVING,
        PLAYER_MELEE_ATTACK,
        BASICSLIME_IDLE,
        BASICSLIME_JUMP,
        FIRESLIME_IDLE
    }

    public static string StateToString(AnimationStates state) {
        switch (state) {
            case AnimationStates.PLAYER_IDLE:
                return "Player_Idle";
            case AnimationStates.PLAYER_MOVING:
                return "Player_Moving";
            case AnimationStates.PLAYER_MELEE_ATTACK:
                return "Player_MeleeAttack";
            case AnimationStates.BASICSLIME_IDLE:
                return "BasicSlime_Idle";
            case AnimationStates.BASICSLIME_JUMP:
                return "BasicSlime_Jump";
            case AnimationStates.FIRESLIME_IDLE:
                return "FireSlime_Idle";
            default:
                return null;
        }
    }
}

