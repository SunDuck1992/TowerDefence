using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using YG;
using System.Reflection;

public class BuildScreen : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _secondPanel;
    [SerializeField] private List<TextMeshProUGUI> _towerCostTexts;
    [SerializeField] private TextMeshProUGUI _improveLevelText;
    [SerializeField] private List<Image> _towerImages;
    [SerializeField] private Image _currentImage;
    [SerializeField] private int _destroycost;
    [SerializeField] private int _improveCost;

    private BuildTowersSystem _buildTowerSystem;
    private SceneSettings _sceneSettings;
    private PlayerWallet _playerWallet;
    private Tower _tower;
    private Sprite _currentSprite;
    private float _duration = 2f;
    private string _needMoreGoldText = "Need more gold";
    private bool[] _isCoroutineRunning = new bool[4];

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
        for (int i = 0; i < _sceneSettings.BuildPoints.Count; i++)
        {
            for (int j = 0; j < YandexGame.savesData.buildedAreas.Count; j++)
            {
                if (_sceneSettings.BuildPoints[i].name == YandexGame.savesData.buildedAreas[j].name && YandexGame.savesData.buildedAreas[j].isBuilded)
                {
                    Debug.LogWarning("������� ����������");

                    _buildTowerSystem.SetCurrentbuildArea(_sceneSettings.BuildPoints[i]);

                    if (_buildTowerSystem == null)
                    {
                        Debug.LogError("_buildTowerSystem is null");
                        return;
                    }
                    if (_buildTowerSystem.TowerSettings == null)
                    {
                        Debug.LogError("_buildTowerSystem.TowerSettings is null");
                        return;
                    }
                    if (_buildTowerSystem.TowerSettings.Datas == null)
                    {
                        Debug.LogError("_buildTowerSystem.TowerSettings.Datas is null");
                        return;
                    }

                    var prefab = _buildTowerSystem.TowerSettings.Datas[YandexGame.savesData.buildedAreas[j].value].Prefab;
                    if (prefab == null)
                    {
                        Debug.LogError("Prefab is null");
                        return;
                    }

                    if (!_sceneSettings.BuildPoints[i].OnBuild)
                    {
                        Debug.LogWarning("����� ������");
                        _buildTowerSystem.BuildTower(_buildTowerSystem.TowerSettings.Datas[YandexGame.savesData.buildedAreas[j].value].Prefab);
                        _tower = _buildTowerSystem.GetBuildTower();
                        _buildTowerSystem.CurrentBuildArea.SetCurrentTower(_tower);
                        _currentSprite = _buildTowerSystem.TowerSettings.Datas[YandexGame.savesData.buildedAreas[j].value].Sprite;
                    }

                   
                }

            }
                Debug.LogWarning("���������� �� �������");
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
                //YandexGame.savesData.towersType.Add(index);
                //YandexGame.savesData.buildedAreas.Add(_buildTowerSystem.CurrentBuildArea, index);
                //_buildTowerSystem.CurrentBuildArea.TowerType = index;
                //YandexGame.savesData.buildedAreaInfos.Add(new BuildedAreaInfo(_buildTowerSystem.CurrentBuildArea, index));
                //YandexGame.savesData.BuildedAreas.Add(new BuildedAreaInfo(_buildTowerSystem.CurrentBuildArea, index));
                //YandexGame.savesData.Test.Add(_buildTowerSystem.CurrentBuildArea, index);
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
            Debug.Log("������ �������� �� ����������� �����������");

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

    public void OnClickButtonImprove()
    {
        var tower = _buildTowerSystem.CurrentBuildArea.CurrentTower as Tower;

        _buildTowerSystem.CurrentBuildArea.IncreaseImproveLevel();

        if (tower != null && _buildTowerSystem.CurrentBuildArea.ImproveLevel <= _buildTowerSystem.CurrentBuildArea.MaxImproveLevel)
        {
            if (_playerWallet.TrySpendGold(_improveCost))
            {
                tower.ImproveDamage(_buildTowerSystem.CurrentBuildArea.ImproveLevel);
                _improveLevelText.text = _buildTowerSystem.CurrentBuildArea.ImproveLevel.ToString();

                Debug.Log(tower.Damage + " - increase Damage");
            }
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

    public void Test()
    {
        //if (YandexGame.savesData.buildedAreaInfos.Count != 0)
        //{
        //    for (int i = 0; i <= YandexGame.savesData.buildedAreaInfos.Count; i++)
        //    {
        //        Debug.LogWarning(YandexGame.savesData.buildedAreaInfos[i].Area + " - Area, + " + YandexGame.savesData.buildedAreaInfos[i].Value + "  - Value");
        //    }
        //}



        //Debug.LogWarning(YandexGame.savesData.buildedAreaInfos[0].Area + " - Area, + " + YandexGame.savesData.buildedAreaInfos[0].Value + "  - Value");


    }

    //private void ChangeButtonSprite(int index)
    //{
    //    var texts = _buttons[index].GetComponentsInChildren<TextMeshProUGUI>();

    //    foreach (var text in texts)
    //    {
    //        if (text != null)
    //        {
    //            text.gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            text.gameObject.SetActive(true);
    //        }
    //    }

    //    for (int i = 0; i < _buttons.Count; i++)
    //    {
    //        if (_buttons[i] == _buttons[index])
    //        {
    //            if (_isBuyeds[i])
    //            {
    //                _buttons[i].sprite = _buyedSprite;
    //            }
    //            else
    //            {
    //                _buttons[i].sprite = _baseSprite;
    //            }
    //        }
    //    }
    //}

    private IEnumerator ChangeText(int index)
    {
        var nextTexts = _towerCostTexts[index];

        string text = nextTexts.text;

        nextTexts.text = _needMoreGoldText;
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

        _isCoroutineRunning[index] = false;
    }
}
