using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using YG;
using TMPro;
using System;

public class StoreResources : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    [SerializeField] private int _costGem;
    [SerializeField] private TextMeshProUGUI _needMoreText;
    [SerializeField] private List<LocalizationFont> _localizationFonts;

    private PlayerWallet _playerWallet;

    private int _gemCount = 1;
    private float _duration = 2f;
    private Coroutine _coroutine;

    [Inject]
    public void Construct(PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
    }

    public void OnClickButtonSellGem()
    {
        if (_playerWallet.TrySpendGold(_costGem))
        {
            _playerWallet.AddGem(_gemCount);
        }
        else
        {
            _coroutine = StartCoroutine(ChangeText());
        }
    }

    public void OnClickButtonRewardGem(int id)
    {
        YandexGame.RewVideoShow(id);
    }

    private IEnumerator ChangeText()
    {
        var nextTexts = _needMoreText;

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
}

[Serializable]
public class LocalizationFont
{
    public string languageCode;
    public TMP_FontAsset font;
}
