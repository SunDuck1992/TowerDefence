using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LocalizationImage : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    [SerializeField] private Image _englishImage;
    [SerializeField] private Image _russiaImage;
    [SerializeField] private Image _turkishImage;

    private void Awake()
    {
#if !UNITY_EDITOR
        string languageCode = YandexGame.lang;

        switch (languageCode)
        {
            case EnglishCode:
                _englishImage.gameObject.SetActive(true);
                break;

            case RussianCode:
                _russiaImage.gameObject.SetActive(true);
                break;

            case TurkishCode:
                _turkishImage.gameObject.SetActive(true);
                break;

            default:
                _englishImage.gameObject.SetActive(true);
                break;
        }
#endif
    }
}
