using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControlManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [Header("Settings")]
    [Header("General Movement")]
    [SerializeField] private float _mapScrollSensitivity = 0.05f;
    private Vector2 _movementDirection = Vector2.zero;

    [Header("Mouse Movement")]
    //[SerializeField] private float _mouseMoveSensitivity = 0.05f;
    [SerializeField] private float _edgeSize = 10f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        if (!_camera)
        {
            Debug.LogWarning($"CameraControlManager failed to find a camera on <color=cyan>{gameObject.name}</color>.");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        Vector2 dir = Vector2.zero;

        if (_movementDirection != Vector2.zero) //Handles keyboard movement
        {
            dir = _movementDirection;
        }
        else //Handles edge-scrolling movement
        {
            if (Input.mousePosition.x > Screen.width - _edgeSize) dir += Vector2.right;
            else if (Input.mousePosition.x < _edgeSize) dir += Vector2.left;

            if (Input.mousePosition.y > Screen.height - _edgeSize) dir += Vector2.up;
            else if (Input.mousePosition.y < _edgeSize) dir += Vector2.down;
        }

        _camera.transform.position += (Vector3)dir.normalized * _mapScrollSensitivity;
    }


    public void OnMoveCamera(InputAction.CallbackContext context) //Called when moved by WASD using UnityEvents
    {
        _movementDirection = context.ReadValue<Vector2>();
    }
}
