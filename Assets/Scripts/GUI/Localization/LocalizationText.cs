using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;

public class LocalizationText : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    [SerializeField] private TextMeshProUGUI _englishText;
    [SerializeField] private TextMeshProUGUI _russiaText;
    [SerializeField] private TextMeshProUGUI _turkishText;

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
