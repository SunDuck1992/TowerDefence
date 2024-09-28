using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    //[SerializeField] private List<Transform> _points;

    private int _countEnemies = 1;

    private EnemyManager _enemyManager;
    private SceneSettings _sceneSettings;

    public int WaveCount { get; private set; }
    public int MaxCountEnemies { get; private set; }

    [Inject]
    public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings)
    {
        _enemyManager = enemyManager;
        _sceneSettings = sceneSettings;
    }

    public void SpawnOnClick()
    {
        Debug.Log(_enemyManager + " - EnemyManager");

        Transform point = _sceneSettings.Points[Random.Range(0, _sceneSettings.Points.Count)];

        for (int i = 0; i < _countEnemies; i++)
        {
            _enemyManager.Create(point.position);
        }

        MaxCountEnemies = _countEnemies;
        WaveCount++;
        _countEnemies += 1;
    }
}
