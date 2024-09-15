using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class WaveScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countWavetext;
    [SerializeField] private TextMeshProUGUI _countEnemiesProgressText;
    [SerializeField] private Slider _progressWaveBar;

    private EnemyManager _enemyManager;
    private Spawner _spawner;

    public UnityEvent WaveComplete;
    public event Action OnEndBattle;


    [Inject]
    public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings)
    {
        _enemyManager = enemyManager;
        _spawner = sceneSettings.Spawner;
    }

    private void Start()
    {
        _enemyManager.EnemyDied += UpdateProgressBar;
    }

    private void OnDestroy()
    {
        _enemyManager.EnemyDied -= UpdateProgressBar;
    }

    public void StartBattle()
    {
        _spawner.SpawnOnClick();
        _countEnemiesProgressText.text = $"{0} / {_spawner.MaxCountEnemies}";
        _countWavetext.text = _spawner.WaveCount.ToString();
        _progressWaveBar.maxValue = _spawner.MaxCountEnemies;
        _progressWaveBar.value = 0;        
    }

    private void UpdateProgressBar()
    {
        _progressWaveBar.value++;
        _countEnemiesProgressText.text = $"{_progressWaveBar.value} / {_spawner.MaxCountEnemies}";

        if(_progressWaveBar.value >= _spawner.MaxCountEnemies)
        {
            WaveComplete.Invoke();
            OnEndBattle?.Invoke();
        }
    }    
}
