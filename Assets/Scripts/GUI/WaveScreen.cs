using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using YG;
using YG.Utils.LB;


public class WaveScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countWavetext;
    [SerializeField] private TextMeshProUGUI _countEnemiesProgressText;
    [SerializeField] private Slider _progressWaveBar;
    [SerializeField] private GameObject _backGroundMusic;

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
        ShowBuildAreas();

        _enemyManager.EnemyDied += UpdateProgressBar;
        OnEndBattle += ShowBuildAreas;
        OnEndBattle += _enemyImprover.Improve;
        OnEndBattle += DisableMusic;
        OnEndBattle += SaveLeaderData;
        OnEndBattle += SaveWaweInfo;
    }

    private void OnDestroy()
    {
        _enemyManager.EnemyDied -= UpdateProgressBar;
        OnEndBattle -= ShowBuildAreas;
        OnEndBattle -= _enemyImprover.Improve;
        OnEndBattle -= DisableMusic;
        OnEndBattle -= SaveLeaderData;
        OnEndBattle -= SaveWaweInfo;
    }

    public void StartBattle()
    {
        _spawner.SpawnOnClick();
        _countEnemiesProgressText.text = $"{0} / {_spawner.MaxCountEnemies}";
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

    private void SaveLeaderData()
    {
        YandexGame.savesData.leaderScore += 13;
    }

    private void SaveWaweInfo()
    {
        YandexGame.savesData.waveCount = _spawner.WaveCount;
        YandexGame.savesData.enemyCount = _spawner.CountEnemies;
    }

    private void DisableMusic()
    {
        _backGroundMusic.SetActive(false);
    }
}
