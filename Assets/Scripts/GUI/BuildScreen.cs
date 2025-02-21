using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using YG;
using System.Reflection;
//using System.Diagnostics;

public class BuildScreen : MonoBehaviour
{
    private const string EnglishCode = "en";
    private const string RussianCode = "ru";
    private const string TurkishCode = "tr";

    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _secondPanel;
    [SerializeField] private List<TextMeshProUGUI> _towerCostTexts;
    [SerializeField] private TextMeshProUGUI _improveLevelText;
    [SerializeField] private TextMeshProUGUI _improveCostText;
    [SerializeField] private TextMeshProUGUI _destroyCostText;
    [SerializeField] private List<Image> _towerImages;
    [SerializeField] private Image _currentImage;
    [SerializeField] private Button _increaseButton;
    [SerializeField] private int _destroycost;
    [SerializeField] private int _improveCost;

    [SerializeField] private List<LocalizationFont> _localizationFonts;

    private BuildTowersSystem _buildTowerSystem;
    private SceneSettings _sceneSettings;
    private PlayerWallet _playerWallet;
    private Tower _tower;
    private Sprite _currentSprite;
    private float _duration = 2f;
    //private string _needMoreGoldText = "Need more gold";
    private bool[] _isCoroutineRunning = new bool[5];

    [Inject]
    public void Construct(BuildTowersSystem buildTowersSystem, PlayerWallet playerWallet, SceneSettings sceneSettings)
    {
        _playerWallet = playerWallet;
        _buildTowerSystem = buildTowersSystem;
        _sceneSettings = sceneSettings;
        _buildTowerSystem.InteractBuildArea += ShowBuildScreen;
        _buildTowerSystem.DeInteractBuildArea += HideBuildScreen;
    }

    private void Start()
    {
        _improveCostText.text = _improveCost.ToString();
        _destroyCostText.text = _destroycost.ToString();

        for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
        {
            for (int j = 0; j < YandexGame.savesData.buildedAreas.Count; j++)
            {
                if (_sceneSettings.BuildPoints[i].name == YandexGame.savesData.buildedAreas[j].name && YandexGame.savesData.buildedAreas[j].isBuilded)
                {
                    _buildTowerSystem.SetCurrentbuildArea(_sceneSettings.BuildPoints[i]);

                    if (_buildTowerSystem == null)
                    {
                        return;
                    }
                    if (_buildTowerSystem.TowerSettings == null)
                    {
                        return;
                    }
                    if (_buildTowerSystem.TowerSettings.Datas == null)
                    {
                        return;
                    }

                    var prefab = _buildTowerSystem.TowerSettings.Datas[YandexGame.savesData.buildedAreas[j].value].Prefab;

                    if (prefab == null)
                    {
                        return;
                    }

                    if (!_sceneSettings.BuildPoints[i].OnBuild)
                    {
                        _buildTowerSystem.BuildTower(_buildTowerSystem.TowerSettings.Datas[YandexGame.savesData.buildedAreas[j].value].Prefab);
                        _tower = _buildTowerSystem.GetBuildTower();
                        _buildTowerSystem.CurrentBuildArea.SetCurrentTower(_tower);
                        _currentSprite = _buildTowerSystem.TowerSettings.Datas[YandexGame.savesData.buildedAreas[j].value].Sprite;
                    }
                }
            }
        }
    }

    public void OnClickButtonBuild(int index)
    {
        int costTower = _buildTowerSystem.TowerSettings.Datas[index].Cost;

        if (!_buildTowerSystem.CurrentBuildArea.OnBuild)
        {
            if (_playerWallet.TrySpendGold(costTower))
            {
                _buildTowerSystem.BuildTower(_buildTowerSystem.TowerSettings.Datas[index].Prefab);
                _tower = _buildTowerSystem.GetBuildTower();
                _buildTowerSystem.CurrentBuildArea.SetCurrentTower(_tower);
                _currentSprite = _buildTowerSystem.TowerSettings.Datas[index].Sprite;
                YandexGame.savesData.buildAreas.Add(_buildTowerSystem.CurrentBuildArea);
                YandexGame.savesData.buildedAreas.Add(new BuildedAreaInfo(_buildTowerSystem.CurrentBuildArea.name, index, true));
            }
            else
            {
                if (!_isCoroutineRunning[index])
                {
                    _isCoroutineRunning[index] = true;
                    StartCoroutine(ChangeText(index));
                }
            }
        }
    }

    public void OnClickButtonDestroy(int index)
    {
        if (_buildTowerSystem.CurrentBuildArea.OnBuild)
        {
            if (_playerWallet.TrySpendGold(_destroycost))
            {
                _buildTowerSystem.CurrentBuildArea.DestroyCurrentTower();
            }
            else
            {
                if (!_isCoroutineRunning[index])
                {
                    _isCoroutineRunning[index] = true;
                    StartCoroutine(ChangeText(index));
                }
            }
        }
    }

    public void OnClickButtonImprove(int index)
    {
        var tower = _buildTowerSystem.CurrentBuildArea.CurrentTower as Tower;

        if (tower != null && _buildTowerSystem.CurrentBuildArea.ImproveLevel < _buildTowerSystem.CurrentBuildArea.MaxImproveLevel)
        {
            if (_playerWallet.TrySpendGold(_improveCost))
            {
                _buildTowerSystem.CurrentBuildArea.IncreaseImproveLevel();
                tower.ImproveDamage(_buildTowerSystem.CurrentBuildArea.ImproveLevel);
                _improveLevelText.text = _buildTowerSystem.CurrentBuildArea.ImproveLevel.ToString();
            }
            else
            {
                if (!_isCoroutineRunning[index])
                {
                    _isCoroutineRunning[index] = true;
                    StartCoroutine(ChangeText(index));
                }
            }
        }
        
        if(_buildTowerSystem.CurrentBuildArea.ImproveLevel == _buildTowerSystem.CurrentBuildArea.MaxImproveLevel)
        {
            _increaseButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        _buildTowerSystem.InteractBuildArea -= ShowBuildScreen;
        _buildTowerSystem.DeInteractBuildArea -= HideBuildScreen;
    }

    private void ShowBuildScreen(BuildArea buildArea)
    {
        if (_buildTowerSystem.TowerAreaLocations.ContainsKey(buildArea))
        {
            _secondPanel.SetActive(true);
            _currentImage.sprite = _currentSprite;
            _improveLevelText.text = _buildTowerSystem.CurrentBuildArea.ImproveLevel.ToString();

            if(_buildTowerSystem.CurrentBuildArea.ImproveLevel < _buildTowerSystem.CurrentBuildArea.MaxImproveLevel)
            {
                _increaseButton.interactable = true;
            }
            else
            {
                _increaseButton.interactable = false;
            }
        }
        else
        {
            _panel.SetActive(true);

            for (int i = 0; i < _towerCostTexts.Count; i++)
            {
                _towerCostTexts[i].text = _buildTowerSystem.TowerSettings.Datas[i].Cost.ToString();
            }

            for (int i = 0; i < _towerImages.Count; i++)
            {
                _towerImages[i].sprite = _buildTowerSystem.TowerSettings.Datas[i].Sprite;
            }
        }
    }

    private void ChangeImproveLevel(BuildArea buildArea)
    {
        _improveLevelText.text = _buildTowerSystem.CurrentBuildArea.ImproveLevel.ToString();
    }

    private void HideBuildScreen()
    {
        _panel.SetActive(false);
        _secondPanel.SetActive(false);
    }

    private IEnumerator ChangeText(int index)
    {
        var nextTexts = _towerCostTexts[index];

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

        _isCoroutineRunning[index] = false;
    }

    [Serializable]
    public class LocalizationFont
    {
        public string languageCode;
        public TMP_FontAsset font;
    }
}
