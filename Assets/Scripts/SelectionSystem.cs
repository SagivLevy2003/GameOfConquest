using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionSystem : Singleton<SelectionSystem>
{
    [SerializeField] private bool _debugInput = false;
    [field: SerializeField] public GameObject SelectedObject { get; private set; }
    public UnityEvent<GameObject> OnObjectSelected = new();
    public UnityEvent<GameObject> OnObjectDeselected = new();

    public IEnumerator TrySelectHoveredObjectCoroutine() //Response to player 'Select' action (left click)
    {
        yield return null;
        TrySelectHoveredObject();
    } //IsPointOverGameObject is only relevant to the previous frame, so I am delaying the execution of code by 1 frame to keep them synced

    private void TrySelectHoveredObject() 
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; //Pointer is over a UI element — ignore gameplay click

        GameObject hoveredObject = MouseInputSystem.Instance.GetHoveredObject();

        if (!hoveredObject)
        {
            DeselectCurrentObject();
            return; //Return if not hovering over an object
        }

        ISelectable selectable = hoveredObject.GetComponentInChildren<ISelectable>();

        if (selectable == null)
        {
            DeselectCurrentObject();
            return; //Return if the object isn't selectable
        }

        if (_debugInput) Debug.Log($"Selecting {hoveredObject}");
        SelectObject(hoveredObject, selectable);
    }


    private void SelectObject(GameObject obj, ISelectable selectable) 
    {
        DeselectCurrentObject(); //Deselects previous object
        SelectedObject = obj; //Assigns the object
        selectable.OnSelect(); //Calls the selectable object's selection method

        OnObjectSelected?.Invoke(obj); //Invokes the global selection event
    }

    private void DeselectCurrentObject()
    {
        if (SelectedObject == null) return; //Return if no object is hovered

        if (_debugInput) Debug.Log($"Deselecting {SelectedObject}");

        SelectedObject.GetComponent<ISelectable>()?.OnDeselect(); //Calls the selectable object's deselection method
        SelectedObject = null;

        OnObjectDeselected?.Invoke(SelectedObject); //Invokes the global deselection event
    }
}
