using UnityEngine;

[CreateAssetMenu(menuName = "Unit Training Data")]
public class UnitTrainingData : ScriptableObject
{
    [field: SerializeField] public float MinManpowerCost { get; private set; }
    [field: SerializeField] public float MaxManpowerCost { get; private set; }
    [field: SerializeField] public string UnitName { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    //Add icons, sounds etc 
}