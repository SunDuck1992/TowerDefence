using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using YG;

public class WaveScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countWavetext;
    [SerializeField] private TextMeshProUGUI _countEnemiesProgressText;
    [SerializeField] private Slider _progressWaveBar;
    //[SerializeField] private List<BuildArea> _buildAreas;

    private EnemyManager _enemyManager;
    private Spawner _spawner;
    private SceneSettings _sceneSettings;
    private EnemyImprover _enemyImprover;

    public UnityEvent WaveComplete;
    public event Action OnEndBattle;


    [Inject]
    public void Construct(EnemyManager enemyManager, SceneSettings sceneSettings, EnemyImprover enemyImprover)
    {
        _enemyManager = enemyManager;
        _spawner = sceneSettings.Spawner;
        _sceneSettings = sceneSettings;
        _enemyImprover = enemyImprover;
    }

    private void Start()
    {
        _enemyManager.EnemyDied += UpdateProgressBar;
        OnEndBattle += ShowBuildAreas;
        OnEndBattle += _enemyImprover.Improve;
    }

    private void OnDestroy()
    {
        _enemyManager.EnemyDied -= UpdateProgressBar;
        OnEndBattle -= ShowBuildAreas;
        OnEndBattle -= _enemyImprover.Improve;
    }

    public void StartBattle()
    {
        _spawner.SpawnOnClick();
        _countEnemiesProgressText.text = $"{0} / {_spawner.MaxCountEnemies}";

        //if(YandexGame.savesData.waveCount == -1)
        //{
        //    _countWavetext.text = _spawner.WaveCount.ToString();
        //}
        //else
        //{
        //    _countWavetext.text = YandexGame.savesData.waveCount.ToString();
        //}

        _countWavetext.text = _spawner.WaveCount.ToString();
        _progressWaveBar.maxValue = _spawner.MaxCountEnemies;
        _progressWaveBar.value = 0;

        for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
        {
            _sceneSettings.BuildPoints[i].gameObject.SetActive(false);
        }
    }

    private void UpdateProgressBar()
    {
        _progressWaveBar.value++;
        _countEnemiesProgressText.text = $"{_progressWaveBar.value} / {_spawner.MaxCountEnemies}";

        if (_progressWaveBar.value >= _spawner.MaxCountEnemies)
        {
            WaveComplete.Invoke();
            OnEndBattle?.Invoke();
        }
    }

    private void ShowBuildAreas()
    {
        if (_progressWaveBar.value >= _spawner.MaxCountEnemies)
        {
            for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
            {
                if (_sceneSettings.BuildPoints[i].WaveLevel <= _spawner.WaveCount)
                {
                    _sceneSettings.BuildPoints[i].gameObject.SetActive(true);
                }
            }
        }       
    }
}
