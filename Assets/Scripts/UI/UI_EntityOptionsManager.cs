using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class UI_EntityOptionsManager : MonoBehaviour
{
    [SerializeField] Transform _container;
    [SerializeField] private List<UI_EntityOptionSectionBase> _entityOptionSection = new();

    [Header("Debug Information")]
    [SerializeField, ReadOnly] private List<GameObject> _instantiatedSlotList = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        SelectionSystem.Instance.OnObjectSelected.AddListener(UpdateUIForSelectedObject);
        SelectionSystem.Instance.OnObjectDeselected.AddListener(UpdateUIForSelectedObject);
    }

    private void OnDisable()
    {
        SelectionSystem.Instance.OnObjectSelected.RemoveListener(UpdateUIForSelectedObject);
        SelectionSystem.Instance.OnObjectDeselected.RemoveListener(UpdateUIForSelectedObject);
    }

    public void UpdateUIForSelectedObject(NetworkObject obj)
    {
        ClearMenu();

        if (obj == null) return;

        if (!obj.IsOwner) return;


        foreach (var section in _entityOptionSection)
        {
            section.GenerateSection(obj, this);
        }
    }

    public GameObject CreateSlot(GameObject slotPrefab) //Can be overhauled to create categories later down the line
    {
        if (slotPrefab == null)
        {
            Debug.LogWarning($"Attempted to create a slot with a null prefab!");
            return null;
        }

        GameObject obj = Instantiate(slotPrefab, _container);
        _instantiatedSlotList.Add(obj);

        return obj;
    }

    public void DeleteSlot(GameObject slot)
    {
        if (slot == null)
        {
            Debug.LogWarning($"Attempted to delete a null slot!");
            return;
        }

        if (!_instantiatedSlotList.Contains(slot))
        {
            Debug.LogWarning($"Attempted to delete a slot that isn't assigned in the OptionsManager: <color=cyan>{slot.name}</color>");
            return;
        }

        _instantiatedSlotList.Remove(slot);
        Destroy(slot);
    }

    private void ClearMenu()
    {
        foreach(GameObject slot in _instantiatedSlotList)
        {
            Destroy(slot);
        }

        _instantiatedSlotList.Clear();
    }
}
