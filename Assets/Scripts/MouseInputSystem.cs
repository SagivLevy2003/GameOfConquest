using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputSystem : Singleton<MouseInputSystem>
{
    public void OnSelect(InputAction.CallbackContext context) //Select Action Callback
    {
        if (!context.performed) return; //Ensures the code is only called once per interaction
        StartCoroutine(SelectionSystem.Instance.TrySelectHoveredObjectCoroutine());
    }

    public void OnTarget(InputAction.CallbackContext context) //Target Action Callback
    {
        if (!context.performed) return; //Ensures the code is only called once per interaction
        CommandSystemManager.Instance.TryExecuteCurrentCommandOnServer();
    }

    public GameObject GetHoveredObject() //Returns an object with a collider that the mouse is hovering over
    {
        if (!Camera.main)
        {
            Debug.LogWarning("Camera.main is set to null!");
            return null;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = Physics2D.OverlapPoint(mousePos);

        return collider ? collider.gameObject : null;
    }
}
