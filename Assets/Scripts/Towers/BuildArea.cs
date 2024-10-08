using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BuildArea : MonoBehaviour
{
    [SerializeField] private Transform _buildPoint;
    [SerializeField] private Image _sliderImage;
    [SerializeField] private int _waveLevel;

    private float _fillDuration = 2f;
    private float _pingPongScaleDuration = 1f;
    private float _pingPongHalfScaleDuration = 0.5f;

    private Vector3 _originalScale;
    private Vector3 _pingPongDesiredScale;
    private Vector3 _pingPongScaleAddition = new Vector3(0.2f, 0.2f, 0);

    private Canvas _canvas;
    private Transform _canvasTransform;
    private Coroutine _coroutine;
    private Tower _currentTower;

    private bool _isEnter;

    public Canvas Canvas => _canvas;
    public Transform BuildPoint => _buildPoint;
    public Tower CurrentTower => _currentTower;
    public bool OnBuild { get; set; }
    public BuildTowersSystem BuildTowersSystem { get; set; }
    public int WaveLevel => _waveLevel;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _canvasTransform = _canvas.transform;
        _originalScale = _canvasTransform.localScale;
        _pingPongDesiredScale = _originalScale + _pingPongScaleAddition;

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
        _isEnter = false;

        StopFilling();

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        BuildTowersSystem.OnDeInteractBuildArea();
    }

    public void SetCurrentTower(Tower tower)
    {
        _currentTower = tower;
    }

    public void DestroyCurrentTower()
    {
        Debug.Log(_currentTower.name);
        _currentTower.DiedStart?.Invoke(_currentTower);

    }

    private void StartFilling()
    {
        _sliderImage.fillAmount = 0;

        _sliderImage.DOFillAmount(1f, _fillDuration).OnComplete(() =>
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
