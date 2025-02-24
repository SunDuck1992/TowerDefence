using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using YG;
using Zenject;

public class BuildArea : MonoBehaviour
{
    [SerializeField] private Transform _buildPoint;
    [SerializeField] private Image _sliderImage;
    [SerializeField] private int _waveLevel;

    private float _fillDuration = 2f;
    private float _pingPongScaleDuration = 1f;
    private float _pingPongHalfScaleDuration = 0.5f;

    private int _improveLevel = 1;
    private int _maxImproveLevel = 5;

    private Vector3 _originalScale;
    private Vector3 _pingPongDesiredScale;
    private Vector3 _pingPongScaleAddition = new Vector3(0.2f, 0.2f, 0);

    private Canvas _canvas;
    private Transform _canvasTransform;
    private Coroutine _coroutine;
    private Tower _currentTower;
    private SceneSettings _sceneSettings;

    private bool _isEnter;

    public Canvas Canvas => _canvas;
    public Transform BuildPoint => _buildPoint;
    public Tower CurrentTower => _currentTower;
    public bool OnBuild { get; set; }
    public BuildTowersSystem BuildTowersSystem { get; set; }
    public int WaveLevel => _waveLevel;
    public int ImproveLevel => _improveLevel;
    public int MaxImproveLevel => _maxImproveLevel;

    [Inject]
    public void Construct(SceneSettings sceneSettings)
    {
        _sceneSettings = sceneSettings;
    }


    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _canvasTransform = _canvas.transform;
        _originalScale = _canvasTransform.localScale;
        _pingPongDesiredScale = _originalScale + _pingPongScaleAddition;
    }

    private void Start()
    {
        for (int i = 0; i < YandexGame.savesData.buildedAreas.Count; i++)
        {
            for (int j = 0; j < _sceneSettings.BuildPoints.Count; j++)
            {
                if (YandexGame.savesData.buildedAreas[i].name == _sceneSettings.BuildPoints[j].name)
                {
                    if (YandexGame.savesData.buildedAreas[i].improveLevel != -1)
                    {
                        _sceneSettings.BuildPoints[j]._improveLevel = YandexGame.savesData.buildedAreas[i].improveLevel;
                    }
                }
            }
        }

        if (YandexGame.savesData.UpgradeLevelTower != -1)
        {
            _improveLevel = YandexGame.savesData.UpgradeLevelTower;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _isEnter = true;

        StartFilling();

        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(PingPongScale(_isEnter));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopFilling();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        if (_isEnter)
        {
            BuildTowersSystem.OnDeInteractBuildArea();
            _isEnter = false;
        }
    }

    public void SetCurrentTower(Tower tower)
    {
        _currentTower = tower;
    }

    public void DestroyCurrentTower()
    {
        _currentTower.DiedStart?.Invoke(_currentTower);
    }

    public void IncreaseImproveLevel(BuildTowersSystem buildTowersSystem)
    {
        for (int i = 0; i < YandexGame.savesData.buildedAreas.Count; i++)
        {
            if (YandexGame.savesData.buildedAreas[i].name == buildTowersSystem.CurrentBuildArea.name)
            {
                if (YandexGame.savesData.buildedAreas[i].improveLevel == -1)
                {
                    _improveLevel++;
                    YandexGame.savesData.buildedAreas[i].improveLevel = _improveLevel;
                }
                else
                {
                    if (_improveLevel < _maxImproveLevel)
                    {
                        _improveLevel++;
                        YandexGame.savesData.buildedAreas[i].improveLevel++;
                    }
                }
            }
        }
    }

    private void StartFilling()
    {
        _sliderImage.fillAmount = 0;

        _sliderImage.DOFillAmount(1f, _fillDuration).OnKill(() =>
        {
            BuildTowersSystem.OnInteractBuildArea(this);
            StopCoroutine(_coroutine);
        });
    }

    private void StopFilling()
    {
        _sliderImage.DOKill();
        _sliderImage.fillAmount = 0;
    }

    private IEnumerator PingPongScale(bool isEnter)
    {
        while (isEnter)
        {
            _canvasTransform.DOScale(_pingPongDesiredScale, _pingPongHalfScaleDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                _canvasTransform.DOScale(_originalScale, _pingPongHalfScaleDuration).SetEase(Ease.OutBounce);
            });

            yield return new WaitForSeconds(_pingPongScaleDuration);
        }
    }
}
