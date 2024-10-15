using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ShootRocketTowerState : BaseState<RocketTower>
{
    public GameUnit target;

    private Rocket _rocket;
    private float _multyplieMassiveDamage = 0.5f;

    public override void Enter()
    {
        Debug.LogWarning("Вошли в состояние стрельбы");
        PushRocket();
        Debug.LogWarning("Запустили корутину на спавн ракеты");
    }

    private bool IsSpawnAreaClear()
    {
        Collider[] colliders = Physics.OverlapSphere(Owner.ShotPoint.position, 1f);
        return !colliders.Any(c => c.GetComponent<Rocket>() != null);
    }

    private void OnHit(Enemy enemy)
    {
        enemy.TakeDamage(_rocket.Damage);

        var enemies = enemy.TargetController.GetAllTargets(enemy, 3f, true);

        foreach (var unit in enemies)
        {
            if (unit != enemy)
            {
                unit.TakeDamage(_rocket.Damage * _multyplieMassiveDamage);
            }
        }
    }

    private void RocketComplete(Rocket rocket)
    {
        _rocket.HitTower -= OnHit;
        _rocket.Died -= RocketComplete;
    }

    private void PushRocket()
    {
        if (IsSpawnAreaClear())
        {
            PrepareRocket();
        }

        Owner.StateMachine.SwitchState<ReloadRocketTowerState, RocketTower>(Owner);
    }

    private void PrepareRocket()
    {
        _rocket = Owner.RocketPool.Spawn();
        _rocket.Target = target;
        _rocket.Damage = Owner.Damage;
        _rocket.transform.position = Owner.ShotPoint.position;
        _rocket.transform.forward = Owner.ShotPoint.forward;

        Debug.LogWarning("Заспавнили ракету - " + _rocket.name);

        _rocket.HitTower += OnHit;
        _rocket.Died += RocketComplete;
    }

    //private IEnumerator SpawnRocketWithDelay()
    //{

    //    if (IsSpawnAreaClear())
    //    {
    //        Debug.LogWarning("Прошли проверку на спавн");
    //        PrepareRocket();
    //    }
    //    else
    //    {
    //        yield return new WaitForSeconds(0.2f);

    //        if (IsSpawnAreaClear())
    //        {
    //            Debug.LogWarning("Прошли проверку на спавн со 2 раза");
    //            PrepareRocket();
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Не удалось спавнить ракету из-за препятствий в зоне спавна");
    //        }
    //    }


    //    yield return new WaitForSeconds(1f);

    //    //CoroutineManager.StopStaticCoroutine(_coroutine);
    //    Debug.LogWarning("Перехожу в состояние презарядки");
    //    Owner.StateMachine.SwitchState<ReloadRocketTowerState, RocketTower>(Owner);
    //}
}
