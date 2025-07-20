using FishNet;
using FishNet.Object;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestUnitSpawner : NetworkBehaviour
{
    public GameObject UnitPrefab;
    public GameObject CityPrefab;

    [SerializeField] private Button _unitBtn;
    [SerializeField] private Button _cityBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _unitBtn.onClick.AddListener(() => TrySpawn(InstanceFinder.ClientManager.Connection.ClientId, false));
        _cityBtn.onClick.AddListener(() => TrySpawn(InstanceFinder.ClientManager.Connection.ClientId, true));
    }

    [ServerRpc(RequireOwnership = false)]
    void TrySpawn(int id, bool city)
    {
        GameObject prefab = UnitPrefab;
        if (city) prefab = CityPrefab;

        EntitySpawnFactory.SpawnEntityAtPosition(prefab, id, new(Random.Range(-3,3),Random.Range(-3, 3)));
    }
}
