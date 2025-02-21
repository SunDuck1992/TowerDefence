using System.Collections.Generic;
using UnityEngine;

public abstract class BasePool<T>
    where T : MonoBehaviour
{
    private readonly T _prefab;
    private Queue<T> _storage;
    private bool _isDebug;

    public BasePool(T prefab, bool isDebug = false)
    {
        _prefab = prefab;
        _storage = new Queue<T>();
        _isDebug = isDebug;
    }

    public T Spawn()
    {
        T item = null;

        if (_storage.Count > 0)
        {
            item = _storage.Dequeue();
        }
        else
        {
            item = MonoBehaviour.Instantiate(_prefab);
        }

        OnSpawn(item);
        return item;
    }

    public void Despawn(T despawnObject)
    {
        OnDespawn(despawnObject);
        _storage.Enqueue(despawnObject);
    }

    protected abstract void OnSpawn(T spawnObject);

    protected abstract void OnDespawn(T despawnObject);
}
