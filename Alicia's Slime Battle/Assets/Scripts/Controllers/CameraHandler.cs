using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class, combined with Cinemachine, creates a smooth camera focus effect towards the mouse pointer
 */
public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float followDistance;

    private Vector3 followDirection;

    private void Update() {
        // 这个 class 总的来说就是使玩家可以使用鼠标达成一种丝滑地扩张视野的效果（view extension at aiming direction）
        // Cinemachine 的摄像头不会追随玩家本体，而是这个 class 所附着的 game object（camera target position）
        // 以下代码的作用就是使这个 game object 跟随鼠标指针，但是无法离玩家太远（最大距离的限制就是上面的 [SerializeField] private float followDistance）

        // Gets the direction vector from the player to the mouse pointer
        followDirection = CameraLocator.Instance.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - PlayerLocator.Instance.transform.position;

        // Clamps the max distance of camera away from player to within the follow distance
        if (followDirection.magnitude > followDistance)
            followDirection = followDirection.normalized * followDistance;

        // Moves the camera target position towards the mouse
        transform.position = PlayerLocator.Instance.transform.position + followDirection;
    }
}
