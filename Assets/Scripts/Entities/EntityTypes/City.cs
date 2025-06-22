using System.Collections;
using Unity;
using UnityEngine;

[RequireComponent(typeof(UnitTrainingManager))]
public class City : Structure
{
    [Header("Production")]
    [SerializeField] int _manpowerGenerationPerTick = 1;
    [SerializeField] float _productionTickRate = 1;
    [SerializeField] UnitTrainingManager _trainingManager;

    private void Start()
    {
        _trainingManager = GetComponent<UnitTrainingManager>();
        StartCoroutine(GenerateManpower());
    }

    private IEnumerator GenerateManpower()
    {
        yield return new WaitForSeconds(_productionTickRate);
        AddManpower(_manpowerGenerationPerTick);

        StartCoroutine(GenerateManpower());
    }
}
