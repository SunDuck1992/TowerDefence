using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResourcesScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldValueText;
    [SerializeField] private TextMeshProUGUI _gemValueText;
    [SerializeField] private float _duration;

    private PlayerWallet _playerWallet;
    private int _currentGold;
    private int _targetGold;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }

    private void Start()
    {
        _goldValueText.text = _playerWallet.Gold.ToString();
        _gemValueText.text = _playerWallet.Gem.ToString();

        _playerWallet.GoldChanged += UpdateGoldDisplay;
        _playerWallet.GemChanged += UpdateResourcesScreen;
    }

    private void UpdateResourcesScreen()
    {
        _gemValueText.text = _playerWallet.Gem.ToString();
    }

    private void UpdateGoldDisplay(int targetGold)
    {
        StartCoroutine(UpdateGoldCourutine(targetGold));
    }

    private IEnumerator UpdateGoldCourutine(int targetGold)
    {
        _currentGold = int.Parse(_goldValueText.text);
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            int newGoldValue = Mathf.RoundToInt(Mathf.Lerp(_currentGold, targetGold, elapsedTime / _duration));
            _goldValueText.text = newGoldValue.ToString();

            yield return null;
        }

        _goldValueText.text = targetGold.ToString();
    }

    private void OnDestroy()
    {
        _playerWallet.GemChanged -= UpdateResourcesScreen;
        _playerWallet.GoldChanged -= UpdateGoldDisplay;
    }
}
