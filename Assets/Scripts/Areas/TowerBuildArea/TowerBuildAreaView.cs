using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TowerBuildAreaView : MonoBehaviour
{
    [SerializeField] private float _scaleChangeX;
    [SerializeField] private float _scaleChangeZ;

    private TowerBuildArea _towerBuildArea;
    private Vector3 _scaleChange;
    private Transform _transform;

    //[SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] TowerSelectionView _towerSelectionView;
    [SerializeField] Image _fillImage;
    [SerializeField] private float _pingPongHalfScaleDuration = 0.5f;
    [SerializeField] private Vector3 _pingPongScaleAddition = new Vector3(0.2f, 0.2f, 0);

    private Canvas _canvas;
    private Transform _canvasTransform;
    private Coroutine _pingPongScale;
    private Vector3 _originalScale;
    private Vector3 _pingPongDesiredScale;
    private float _pingPongScaleDuration;

    private void Awake()
    {
        _transform = transform;
        _scaleChange = new Vector3(_scaleChangeX, 0, _scaleChangeZ);
        _towerBuildArea = GetComponentInParent<TowerBuildArea>();

        _canvas = GetComponentInChildren<Canvas>();
    }

    private void OnEnable()
    {
        _towerBuildArea.IsDelivering += OnDelivering;
        _towerBuildArea.GoldDelivering += OnGoldDelivering;
        _towerBuildArea.GoldDelivered += OnGoldDelivered;
    }

    private void OnDisable()
    {
        _towerBuildArea.IsDelivering -= OnDelivering;
        _towerBuildArea.GoldDelivering -= OnGoldDelivering;
        _towerBuildArea.GoldDelivered -= OnGoldDelivered;
    }

    private void Start()
    {
        _canvasTransform = _canvas.transform;
        _originalScale = _canvasTransform.localScale;
        _pingPongDesiredScale = _originalScale + _pingPongScaleAddition;
        _pingPongScaleDuration = _pingPongHalfScaleDuration * 2f;
    }

    private void OnDelivering(bool isDelivering)
    {
        if (isDelivering)
        {
            _pingPongScale = StartCoroutine(PingPongScale(isDelivering));
        }
        else
        {
            StopCoroutine(_pingPongScale);
        }
    }

    private void OnGoldDelivered()
    {
        //_particleSystem.Play();
        Time.timeScale = 0f;
        _towerSelectionView.gameObject.SetActive(true);
    }

    private void OnGoldDelivering(int currentGoldToDelive, int goldToDelive)
    {
        _fillImage.fillAmount = (float)currentGoldToDelive / goldToDelive;
    }

    private IEnumerator PingPongScale(bool isDelivering)
    {
        while (isDelivering)
        {
            _canvasTransform.DOScale(_pingPongDesiredScale, _pingPongHalfScaleDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                _canvasTransform.DOScale(_originalScale, _pingPongHalfScaleDuration).SetEase(Ease.OutBounce);
            });

            yield return new WaitForSeconds(_pingPongScaleDuration);
        }
    }
}
