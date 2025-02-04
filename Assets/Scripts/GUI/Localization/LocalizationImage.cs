using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
                _englishText.gameObject.SetActive(true);
                break;

            case RussianCode:
                _russiaText.gameObject.SetActive(true);
                break;

            case TurkishCode:
                _turkishText.gameObject.SetActive(true);
                break;

            default:
                _englishText.gameObject.SetActive(true);
                break;
        }
#endif
    }
}
