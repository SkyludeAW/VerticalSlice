using UnityEngine;

// Objects with this class always faces the camera
public class Billboard : MonoBehaviour {
    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    private void Update() {
        transform.LookAt(_camera.transform);
    }
}
