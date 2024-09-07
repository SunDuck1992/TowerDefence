using UnityEngine;

public class TowerSelectionView : MonoBehaviour
{
    private TowerBuildArea _towerBuildArea;
    private GameObject _tower;

    private void Awake()
    {
        _towerBuildArea = GetComponentInParent<TowerBuildArea>();
    }

    public void BuildTower(GameObject tower)
    {
        tower.gameObject.SetActive(true);
        _towerBuildArea.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
