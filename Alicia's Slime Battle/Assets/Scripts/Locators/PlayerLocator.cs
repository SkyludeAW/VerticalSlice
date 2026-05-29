using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Singleton class for locating the player
 */
public class PlayerLocator : MonoBehaviour
{
    public static PlayerLocator Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this)
            // 你是被命运残酷抛弃的弃子，注定无法在这 Singleton 之中存在!
            Destroy(this);
        else
            // 你即为天选之子，Singleton 的唯一 Instance！
            Instance = this;
    }
}
