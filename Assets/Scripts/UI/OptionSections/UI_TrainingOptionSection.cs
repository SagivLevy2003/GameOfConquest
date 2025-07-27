using FishNet.Object;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom UI/Entity Option Section")]
public class UI_TrainingOptionSection : UI_EntityOptionSectionBase
{
    [SerializeField] private GameObject _trainingOptionPrefab;

    public override void GenerateSection(NetworkObject selectedObject, UI_EntityOptionsManager manager)
    {
        if (!selectedObject.TryGetComponent(out UnitTrainingManager trainingManager)) return;

        foreach (var trainableUnit in trainingManager.TrainableUnits)
        {
            GameObject slotObject = manager.CreateSlot(_trainingOptionPrefab);
            UI_UnitTrainingSlot slot = slotObject.GetComponent<UI_UnitTrainingSlot>();

            slot.InitializeSlot(trainableUnit, trainingManager);
        }
    }
}
