using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopScreen : MonoBehaviour
{
    [SerializeField] private List<ItemSettings> _items;
    [SerializeField] private List<Image> _buttons;
    [SerializeField] private Sprite _inHandSprite;
    [SerializeField] private Sprite _buyedSprite;
    [SerializeField] private List<TextMeshProUGUI> _costTexts;

    private PlayerShooter _playerShooter;
    private PlayerWallet _playerWallet;
    private string _needMoreGoldText = "Need more gold";

    [Inject]
    public void Construct(PlayerShooter playerShooter, PlayerWallet playerWallet)
    {
        _playerShooter = playerShooter;
        _playerWallet = playerWallet;
    }

    private void Start()
    {
        LoadSettings();
    }

    public void ChangeWeaponButtonClick(int index)
    {
        var item = _items[index];

        if (item.isBuyed)
        {
            _playerShooter.ChangeWeapon(index);
            ChangeButtonSprite(index);
        }
        else
        {
            if (_playerWallet.TrySpendGold(item.cost))
            {
                _playerShooter.ChangeWeapon(index);
                ChangeButtonSprite(index);
                item.isBuyed = true;
            }
            else
            {
                StartCoroutine(ChangeText(index));
            }
        }
    }

    private void ChangeButtonSprite(int index)
    {
        _buttons[index].sprite = _inHandSprite;
        var texts = _buttons[index].gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var text in texts)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < _buttons.Count; i++)
        {
            if (_buttons[i] != _buttons[index])
            {
                if (_items[i].isBuyed)
                {
                    _buttons[i].sprite = _buyedSprite;
                }
            }
        }
    }

    private void LoadSettings()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].isBuyed)
            {
                ChangeButtonSprite(i);
            }
        }
    }

    private IEnumerator ChangeText(int index)
    {
        var nextTexts = _costTexts[index];

        string text = nextTexts.text;

        nextTexts.text = _needMoreGoldText;
        nextTexts.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        for (float t = 2f; t >= 0; t -= Time.deltaTime)
        {
            Color color = nextTexts.color;
            color.a = t;
            nextTexts.color = color;

            yield return null;
        }

        nextTexts.text = text;
        nextTexts.color = Color.white;
    }



    [Serializable]
    private class ItemSettings
    {
        public int cost;
        public bool isBuyed;
    }
}


