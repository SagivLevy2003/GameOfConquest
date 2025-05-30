using UnityEngine;

public class SelectionSystem : Singleton<SelectionSystem>
{
    [SerializeField] private bool _debugInput = false;
    [field: SerializeField] public GameObject SelectedObject { get; private set; }

    public void TrySelectHoveredObject() //Response to player 'Select' action (left click)
    {
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

    void SelectObject(GameObject obj, ISelectable selectable) 
    {
        DeselectCurrentObject();
        SelectedObject = obj;
        selectable.OnSelect(); 
    }

    void DeselectCurrentObject()
    {
        if (SelectedObject == null) return; //Return if no object is hovered

        if (_debugInput) Debug.Log($"Deselecting {SelectedObject}");

        SelectedObject.GetComponent<ISelectable>()?.OnDeselect(); //Deselects the object (assuming it contains an Iselectable script, which it should)
        SelectedObject = null;
    }
}
