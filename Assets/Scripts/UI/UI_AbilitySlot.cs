using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilitySlot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TextMeshProUGUI _nameDisplay;
    [SerializeField] private Button _btn;

    [SerializeField, ReadOnly] private EntityAbility _ability;
    [SerializeField, ReadOnly] private EntityAbilityHandler _abilityHandler;

    private void OnEnable()
    {
        _btn.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _btn.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        _abilityHandler.TryExecuteAbility(_ability);
    }

    public void InitializeSlot(EntityAbility ability, EntityAbilityHandler abilityHandler)
    {
        _nameDisplay.text = ability.DisplayName;
        _ability = ability;
        _abilityHandler = abilityHandler;
    }
}