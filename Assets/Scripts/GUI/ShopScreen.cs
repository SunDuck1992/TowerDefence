using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class ShopScreen : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    [SerializeField] private List<ItemSettings> _items;
    [SerializeField] private List<Image> _buttons;
    [SerializeField] private Sprite _inHandSprite;
    [SerializeField] private Sprite _buyedSprite;
    [SerializeField] private List<TextMeshProUGUI> _costTexts;
    [SerializeField] private Button _miniGunButton;
    [SerializeField] private GameObject _weaponPanel;

    [SerializeField] private List<LocalizationFont> _localizationFonts;

    private PlayerShooter _playerShooter;
    private PlayerWallet _playerWallet;
    private SceneSettings _sceneSettings;
    private Coroutine _coroutine;
    private float _duration = 2f;
    private int _levelForMiniGun = 5;
    //private string _needMoreGoldText = "Need more gold";

    [Inject]
    public void Construct(PlayerShooter playerShooter, PlayerWallet playerWallet, SceneSettings sceneSettings)
    {
        _playerShooter = playerShooter;
        _playerWallet = playerWallet;
        _sceneSettings = sceneSettings;
    }

    public void ChangeWeaponButtonClick(int index)
    {
        var item = _items[index];

        if (item.isBuyed)
        {
            _playerShooter.ChangeWeapon(index);
            ChangeButtonSprite(index);
            YandexGame.savesData.weaponIndex = index;
            _weaponPanel.SetActive(false);
        }
        else
        {
            if (_playerWallet.TrySpendGold(item.cost))
            {
                _playerShooter.ChangeWeapon(index);
                ChangeButtonSprite(index);
                item.isBuyed = true;
                YandexGame.savesData.weaponsIsBuyed[index] = item.isBuyed;
                YandexGame.savesData.weaponIndex = index;
                _weaponPanel.SetActive(false);
            }
            else
            {
               _coroutine = StartCoroutine(ChangeText(index));
            }
        }
    }

    public bool CheckPurchaseItem(int index)
    {
        var item = _items[index];

        return item.isBuyed;
    }

    public void ChangeButtonSprite()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].isBuyed = YandexGame.savesData.weaponsIsBuyed[i];

            if (_items[i].isBuyed == true)
            {
                ChangeButtonSprite(i);
            }
        }

        if (YandexGame.savesData.weaponIndex != -1)
        {
            ChangeButtonSprite(YandexGame.savesData.weaponIndex);
        }
    }

    public void CheckToBuyMiniGun()
    {
        if (_levelForMiniGun <= _sceneSettings.Spawner.WaveCount)
        {
            _miniGunButton.interactable = true;
            Debug.LogWarning(_levelForMiniGun + " - " + _sceneSettings.Spawner.WaveCount);
        }
        else
        {
            Debug.LogWarning("need more level");
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
        TMP_FontAsset asset = nextTexts.font;

#if !UNITY_EDITOR
        string languageCode = YandexGame.lang;

        foreach(LocalizationFont localizationFont in _localizationFonts)
        {
            if(localizationFont.languageCode == languageCode)
            {
                nextTexts.font = localizationFont.font;
            }       
        }

        switch (languageCode)
        {
            case EnglishCode:
                nextTexts.text = "Need more gold";
                break;

            case RussianCode:
                nextTexts.text = "Нужно больше золота";
                break;

            case TurkishCode:
                nextTexts.text = "daha fazla altın lazım";
                break;

            default:
                nextTexts.text = "Need more gold";
                break;
        }
#endif

        //nextTexts.text = _needMoreGoldText;
        nextTexts.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        for (float t = _duration; t >= 0; t -= Time.deltaTime)
        {
            Color color = nextTexts.color;
            color.a = t;
            nextTexts.color = color;

            yield return null;
        }

        nextTexts.text = text;
        nextTexts.color = Color.white;
        nextTexts.font = asset;

        StopCoroutine(_coroutine);
    }

    [Serializable]
    private class ItemSettings
    {
        public int cost;
        public bool isBuyed;
    }

    [Serializable]
    public class LocalizationFont
    {
        public string languageCode;
        public TMP_FontAsset font;
    }
}


