using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildScreen : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _secondPanel;
    [SerializeField] private List<TextMeshProUGUI> _towerCostTexts;
    [SerializeField] private List<Image> _towerImages;
    [SerializeField] private Image _currentImage;
    [SerializeField] private int _destroycost;
    //[SerializeField] private Sprite _buyedSprite;
    //[SerializeField] private Sprite _baseSprite;
    //[SerializeField] private List<bool> _isBuyeds;
    //[SerializeField] private List<Image> _buttons;

    private BuildTowersSystem _buildTowerSystem;
    private PlayerWallet _playerWallet;
    private Tower _tower;
    private Sprite _currentSprite;
    private float _duration = 2f;
    private string _needMoreGoldText = "Need more gold";
    private bool[] _isCoroutineRunning = new bool[4];

    [Inject]
    public void Construct(BuildTowersSystem buildTowersSystem, PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
        _buildTowerSystem = buildTowersSystem;
        _buildTowerSystem.InteractBuildArea += ShowBuildScreen;
        _buildTowerSystem.DeInteractBuildArea += HideBuildScreen;
    }

    public void OnClickButtonBuild(int index)
    {
        int costTower = _buildTowerSystem.TowerSettings.Datas[index].Cost;

        if (!_buildTowerSystem.CurrentBuildArea.OnBuild)
        {
            if (_playerWallet.TrySpendGold(costTower))
            {
                //_isBuyeds[index] = true;
                _buildTowerSystem.BuildTower(_buildTowerSystem.TowerSettings.Datas[index].Prefab);
                _tower = _buildTowerSystem.GetBuildTower();
                _buildTowerSystem.CurrentBuildArea.SetCurrentTower(_tower);
                _currentSprite = _buildTowerSystem.TowerSettings.Datas[index].Sprite;

                //ChangeButtonSprite(index);
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
        if (/*_buildTowerSystem.CurrentBuildArea != null && _buildTowerSystem.CurrentBuildArea.CurrentTower != null*/  _buildTowerSystem.CurrentBuildArea.OnBuild)
        {
            Debug.Log("ѕрошли проверку на возможность уничтожени€");

            if(_playerWallet.TrySpendGold(_destroycost))
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

    //public void OnCkickBool()
    //{
    //    _isBuyeds[0] = false;
    //    ChangeButtonSprite(0);
    //}

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

        //for (int i = 0; i < _towerCostTexts.Count; i++)
        //{
        //    _towerCostTexts[i].text = _buildTowerSystem.TowerSettings.Datas[i].Cost.ToString();
        //}
    }

    private void HideBuildScreen()
    {
        _panel.SetActive(false);
        _secondPanel.SetActive(false);
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
