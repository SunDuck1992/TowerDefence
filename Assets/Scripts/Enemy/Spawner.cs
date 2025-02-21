using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using YG;

public class Spawner : MonoBehaviour
{
    private int _countEnemies;
    private EnemyManager _enemyManager;
    private SceneSettings _sceneSettings;

    public int CountEnemies => _countEnemies;
    public int WaveCount { get; private set; }
    public int MaxCountEnemies { get; private set; }

    [Inject]
    public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings)
    {
        _enemyManager = enemyManager;
        _sceneSettings = sceneSettings;
    }

    private void Start()
    {
        if(YandexGame.savesData.enemyCount == -1)
        {
            _countEnemies = 3;
        }
        else
        {
            _countEnemies = YandexGame.savesData.enemyCount;
        }

        if(YandexGame.savesData.waveCount == -1)
        {
            WaveCount = 0;
        }
        else
        {
            WaveCount = YandexGame.savesData.waveCount;
        }
    }

    public void SpawnOnClick()
    {
        //Transform point = _sceneSettings.Points[Random.Range(0, _sceneSettings.Points.Count)];

        for (int i = 0; i < _countEnemies; i++)
        {
            Transform point = _sceneSettings.Points[Random.Range(0, _sceneSettings.Points.Count)];
            _enemyManager.Create(point.position);
        }

        MaxCountEnemies = _countEnemies;
        WaveCount++;
        _countEnemies += 1;  
    }
}
