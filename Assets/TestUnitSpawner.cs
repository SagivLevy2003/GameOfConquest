using FishNet;
using FishNet.Object;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestUnitSpawner : NetworkBehaviour
{
    public GameObject Prefab;
    private Button _btn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(() => TrySpawn(InstanceFinder.ClientManager.Connection.ClientId));
    }

    [ServerRpc(RequireOwnership = false)]
    void TrySpawn(int id)
    {
        EntitySpawnFactory.SpawnEntityAtPosition(Prefab, id, new(Random.Range(-3,3),Random.Range(-3, 3)));
    }
}
