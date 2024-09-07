using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TowerSettings), menuName = "GameData/" + nameof(TowerSettings))]
public class TowerSettings : ScriptableObject
{
    [SerializeField] private List<TowerData> _datas;

    public IReadOnlyList<TowerData> Datas => _datas;
}

[Serializable]
public struct TowerData
{
    [SerializeField] private Tower _prefab;
    [SerializeField] private int _cost;

    public Tower Prefab => _prefab;
    public int Cost => _cost;
}
