using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController
{
    private List<GameUnit> _enemies = new();
    private List<GameUnit> _towers = new();

    public List<GameUnit> Enemies => _enemies;
    public List<GameUnit> Towers => _towers;

    public void AddTarget(GameUnit gameUnit, bool isTower = false)
    {
        if (isTower)
        {
            _towers.Add(gameUnit);
        }
        else
        {
            _enemies.Add(gameUnit);
        }
    }

    public void RemoveTarget(GameUnit gameUnit)
    {
        _enemies.Remove(gameUnit);
        _towers.Remove(gameUnit);
    }

    public GameUnit GetTarget(GameUnit gameUnit, float radius, bool isTower = false)
    {
        if (isTower)
        {
            return CommonHandler.Find(_enemies, gameUnit.transform, radius);
        }
        else
        {
            List<GameUnit> targets = CommonHandler.FindAll(_towers, gameUnit.transform, radius);

            GameUnit target = null;

            float minDistance = float.PositiveInfinity;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].AttackSector.freePoints.Count > 0)
                {
                    float distance = Vector3.Distance(gameUnit.transform.position, targets[i].transform.position);

                    if (distance < minDistance)
                    {
                        target = targets[i];
                        minDistance = distance;
                    }
                }
            }

            return target;
        }
    }

    public IReadOnlyList<GameUnit> GetAllTargets(GameUnit gameUnit, float radius, bool isTower = false)
    {
        if (isTower)
        {
            return GetTargetsInRange(gameUnit.transform, radius, _enemies);
        }
        else
        {
            return GetTargetsInRange(gameUnit.transform, radius, _towers);
        }
    }

    private List<GameUnit> GetTargetsInRange(Transform transform, float radius, List<GameUnit> units)
    {
        List<GameUnit> targets = new List<GameUnit>();

        for (int i = 0; i < units.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, units[i].transform.position);

            if (distance < radius)
            {
                targets.Add(units[i]);
            }
        }

        return targets;
    }
}
