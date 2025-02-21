using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TowerBuildArea : MonoBehaviour
{
    [SerializeField] private int _goldToDelive;
    [SerializeField] private int _minimumGoldToDelive;
    [SerializeField] private TMP_Text _goldText;

    public event Action GoldDelivered;
    public event Action<int, int> GoldDelivering;
    public event Action<bool> IsDelivering;

    private int _currentGoldToDelive;
    private bool _isDelivering;

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
}
