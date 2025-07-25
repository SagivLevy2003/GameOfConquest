using FishNet.Object;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom UI/Unit Ability Section")]
public class UI_UnitAbilityOptionSection : UI_EntityOptionSectionBase
{
    public override void GenerateSection(NetworkObject selectedObject, UI_EntityOptionsManager manager)
    {
        if (selectedObject == null) return;


    }
}