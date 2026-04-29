using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour {
    public Vector3 Location { get; private set; }

    public bool Invulnerable = false;
    public float Hitpoints = 100f;
}
