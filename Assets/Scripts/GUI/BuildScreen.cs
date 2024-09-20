using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuildScreen : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _tower1CostText;
    [SerializeField] private TextMeshProUGUI _tower2CostText;
    [SerializeField] private TextMeshProUGUI _tower3CostText;

    private BuildTowersSystem _buildTowerSystem;
    private PlayerWallet _playerWallet;

    [Inject]
    public void Construct(BuildTowersSystem buildTowersSystem, PlayerWallet playerWallet)
    {
        _playerWallet = playerWallet;
        _buildTowerSystem = buildTowersSystem;
        _buildTowerSystem.InteractBuildArea += ShowBuildScreen;
        _buildTowerSystem.DeInteractBuildArea += HideBuildScreen;
    }

    public void OnClickButtonBuild(int id)
    {
        int costTower = _buildTowerSystem.TowerSettings.Datas[id].Cost;

        if (_playerWallet.TrySpendGold(costTower))
        {
            _buildTowerSystem.BuildTower(_buildTowerSystem.TowerSettings.Datas[id].Prefab);
        }
        else
        {
            // TODO
            // Добавить экран с недостатком денег
        }
    }

    private void OnDestroy()
    {
        _buildTowerSystem.InteractBuildArea -= ShowBuildScreen;
        _buildTowerSystem.DeInteractBuildArea -= HideBuildScreen;
    }

    private void ShowBuildScreen(BuildArea buildArea)
    {
        _panel.SetActive(true);
        _tower1CostText.text = _buildTowerSystem.TowerSettings.Datas[0].Cost.ToString();
        _tower2CostText.text = _buildTowerSystem.TowerSettings.Datas[1].Cost.ToString();
        _tower3CostText.text = _buildTowerSystem.TowerSettings.Datas[2].Cost.ToString();
    }

    private void HideBuildScreen()
    {
        _panel.SetActive(false);
    }

    
}
