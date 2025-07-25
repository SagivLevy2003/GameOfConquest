using FishNet.Object;
using UnityEngine;

[System.Serializable]
public abstract class UI_EntityOptionSectionBase : ScriptableObject
{
    public abstract void GenerateSection(NetworkObject selectedObject, UI_EntityOptionsManager manager);
}

