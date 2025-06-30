using UnityEngine;

public class Structure : Entity
{
    protected override void OnManpowerDepleted(Entity source = null)
    {
        base.OnManpowerDepleted(source);

        if (source == null) return;

        if (source is Army army) SetOwner(army.OwnerPlayerObject);
    }
}
