using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SceneSettings
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private List<BuildArea> _buldPoints;
    [SerializeField] private Player _player;
    [SerializeField] private BaseUnit _base;
    [SerializeField] private Spawner _spawner;
    
    public IReadOnlyList<BuildArea> BuildPoints => _buldPoints;
    public BaseUnit Base => _base;
    public List<Transform> Points => _points;
    public Player Player => _player;
    public Spawner Spawner => _spawner;
}
