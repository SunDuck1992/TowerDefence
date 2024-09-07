using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameConfig), menuName = "Config/" + nameof(GameConfig))]
public class GameConfig : ScriptableObject
{
    [SerializeField] private PlayerConfig _playerConfig;

    public PlayerConfig PlayerConfig => _playerConfig;
}

public class GameConfigProxy
{
    public readonly GameConfig Config;

    public GameConfigProxy(GameConfig gameConfig)
    {
        Config = gameConfig;
    }
}
