using TMPro;
using UnityEngine;

public class UI_UnitTrainingSlot : MonoBehaviour //An instance of a unit training slot
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshProUGUI _nameDisplay;
    [SerializeField] private TextMeshProUGUI _minimumCostDisplay;

    public void InitializeSlot(UnitTrainingData data)
    {
        //_spriteRenderer.sprite = data
        _nameDisplay.text = data.UnitName;
        _minimumCostDisplay.text = $"Minimum Cost: {data.MinManpowerCost}";
    }
}