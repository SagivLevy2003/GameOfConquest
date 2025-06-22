using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_UnitTrainingBarManager : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _uiContainer;
    [SerializeField] private List<GameObject> _slotList = new();

    private void Start()
    {
        SelectionSystem.Instance.OnObjectSelected.AddListener(TryOpenTrainingMenu);
        SelectionSystem.Instance.OnObjectDeselected.AddListener((_) => ClearMenu());
    }

    private void TryOpenTrainingMenu(GameObject selectedObject)
    {
        ClearMenu();

        if (!selectedObject) return; //Returns if no object is selected

        UnitTrainingManager trainingManager = selectedObject.GetComponent<UnitTrainingManager>(); 
        
        if (!trainingManager) return; //Returns if the selected object is not capable of training units

        //if (trainingManager.OwnerEntity.Owner)

        GenerateMenu(trainingManager);
    }

    private void GenerateMenu(UnitTrainingManager trainingManager) //Generates slots from the selected object's training manager and enables the menu
    {
        foreach (var trainableUnit in trainingManager.TrainableUnits)
        {
            GameObject slotObject = Instantiate(_prefab, _uiContainer.transform);
            UI_UnitTrainingSlot slot = slotObject.GetComponent<UI_UnitTrainingSlot>();
            slot.InitializeSlot(trainableUnit);

            slot.Button.onClick.AddListener(() => trainingManager.TrainUnit(slot.UnitTrainingData));

            _slotList.Add(slotObject);
        }

        _uiContainer.SetActive(true);
    }


    private void ClearMenu() //Deletes all the slots and disables the menu
    {
        foreach (GameObject go in _slotList)
        {
            Destroy(go);
        }

        _uiContainer.SetActive(false);
    }
}
