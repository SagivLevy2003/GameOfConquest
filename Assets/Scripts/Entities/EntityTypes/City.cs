using System.Collections;
using Unity;
using UnityEngine;

[RequireComponent(typeof(UnitTrainingManager))]
public class City : Structure
{
    [Header("Production")]
    [SerializeField] float _currentManpower = 10;
    [SerializeField] float _manpowerGenerationPerTick = 1;
    [SerializeField] float _productionTickRate = 1;
    [SerializeField] UnitTrainingManager _trainingManager;

    public string test => throw new System.NotImplementedException();

    private void Start()
    {
        _trainingManager = GetComponent<UnitTrainingManager>();
        StartCoroutine(GenerateManpower());
    }

    private IEnumerator GenerateManpower()
    {
        yield return new WaitForSeconds(_productionTickRate); 
        _currentManpower += _manpowerGenerationPerTick;
        StartCoroutine(GenerateManpower());
    }
}
