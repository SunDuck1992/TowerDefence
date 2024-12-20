using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject.SpaceFighter;

public class BulletPool : BasePool<Bullet>
{
    public BulletPool(Bullet bullet) : base(bullet)
    {       
    }

    protected override void OnSpawn(Bullet spawnObject)
    {
        spawnObject.gameObject.SetActive(true);
        spawnObject.Died += Despawn;
    }

    protected override void OnDespawn(Bullet despawnObject)
    {
        despawnObject.gameObject.SetActive(false);
        despawnObject.Died -= Despawn;
    }


    //private static BulletPool _instance;

    //public static BulletPool Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new BulletPool();
    //        }
    //        return _instance;
    //    }
    //}
}
