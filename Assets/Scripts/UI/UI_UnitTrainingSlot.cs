using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitTrainingSlot : MonoBehaviour //An instance of a unit training slot
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshProUGUI _nameDisplay;
    [SerializeField] private TextMeshProUGUI _minimumCostDisplay;
    private Button _btn;

    [field: SerializeField, ReadOnly] public UnitTrainingData UnitTrainingData { get; private set; }

    public void InitializeSlot(UnitTrainingData data, UnitTrainingManager trainingManager)
    {
        _btn = GetComponent<Button>();

        //_spriteRenderer.sprite = data
        _nameDisplay.text = data.UnitName;
        _minimumCostDisplay.text = $"Minimum Cost: {data.MinManpowerCost}";
        UnitTrainingData = data;

        _btn.onClick.AddListener(() => trainingManager.TrainUnit(UnitTrainingData));
    }
}
