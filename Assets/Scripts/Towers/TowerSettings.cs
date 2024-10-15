using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = nameof(TowerSettings), menuName = "GameData/" + nameof(TowerSettings))]
public class TowerSettings : ScriptableObject
{
    [SerializeField] private List<TowerData> _datas;

    public IReadOnlyList<TowerData> Datas => _datas;
}

[Serializable]
public struct TowerData
{
    [SerializeField] private string _name;
    [SerializeField] private Tower _prefab;
    [SerializeField] private int _cost;
    [SerializeField] private Sprite _sprite;

    public Tower Prefab => _prefab;
    public int Cost => _cost;
    public Sprite Sprite => _sprite;
}
