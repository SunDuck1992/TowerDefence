using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TowerBuildArea : MonoBehaviour
{
    public event Action GoldDelivered;
    public event Action<int, int> GoldDelivering;
    public event Action<bool> IsDelivering;

    [SerializeField] private int _goldToDelive;
    [SerializeField] private int _minimumGoldToDelive;
    [SerializeField] private TMP_Text _goldText;

    private int _currentGoldToDelive;
    private bool _isDelivering;
    private Coroutine _delive;
    private float _deliveringDelay = 1f;
    private float _deliveringInterval = 0.1f;

    private void Awake()
    {
        _isDelivering = false;
    }

    private void Start()
    {
        _currentGoldToDelive = _goldToDelive;
        _goldText.text = _currentGoldToDelive.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _isDelivering = true;
            IsDelivering?.Invoke(_isDelivering);
            _delive = StartCoroutine(Delive(player));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _isDelivering = false;
            IsDelivering?.Invoke(_isDelivering);
        }
    }

    private IEnumerator Delive(Player player)
    {
        var deliveringDelay = new WaitForSeconds(_deliveringDelay);

        yield return deliveringDelay;

        var deliveringInterval = new WaitForSeconds(_deliveringInterval);

        while (_isDelivering)
        {
            if (_currentGoldToDelive - _minimumGoldToDelive >= 0 && player.TrySpendGold(_minimumGoldToDelive))
            {
                _currentGoldToDelive -= _minimumGoldToDelive;
                _goldText.text = _currentGoldToDelive.ToString();

                GoldDelivering?.Invoke(_currentGoldToDelive, _goldToDelive);
            }

            if (_currentGoldToDelive <= 0)
            {
                GoldDelivered?.Invoke();
            }

            yield return deliveringInterval;
        }
    }
}
