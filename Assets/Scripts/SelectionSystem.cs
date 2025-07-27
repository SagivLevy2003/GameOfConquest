﻿using FishNet;
using FishNet.Object;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionSystem : Singleton<SelectionSystem>
{
    [SerializeField] private bool _debugInput = false;
    [field: SerializeField] public NetworkObject SelectedObject { get; private set; }
    public UnityEvent<NetworkObject> OnObjectSelected = new();
    public UnityEvent<NetworkObject> OnObjectDeselected = new();

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
        NetworkObject netObj = hoveredObject.transform.root.GetComponent<NetworkObject>();

        if (selectable == null || !netObj)
        {
            DeselectCurrentObject();
            return; //Return if the object isn't selectable or networked
        }

        if (_debugInput) Debug.Log($"Selecting {hoveredObject}");
        SelectObject(netObj, selectable);
    }


    private void SelectObject(NetworkObject obj, ISelectable selectable) 
    {
        DeselectCurrentObject(); //Deselects previous object
        SelectedObject = obj;
        selectable.OnSelect();

        //
        //add object despawn event to de-select the object
        //

        OnObjectSelected?.Invoke(obj); 
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
