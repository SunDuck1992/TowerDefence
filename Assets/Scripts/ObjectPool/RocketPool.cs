using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPool : BasePool<Rocket>
{
    public RocketPool(Rocket rocket) : base(rocket)
    {
    }

    protected override void OnSpawn(Rocket spawnObject)
    {
        spawnObject.gameObject.SetActive(true);
        spawnObject.Died += Despawn;
    }

    protected override void OnDespawn(Rocket despawnObject)
    {
        despawnObject.gameObject.SetActive(false);
        despawnObject.Died -= Despawn;
    }
}
