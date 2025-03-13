using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace Script.ObjectPools {
  public class PoolManager : MonoBehaviour {
    [SerializeField]
    private List<PoolEntry> _poolEntries = new List<PoolEntry>();

    private readonly Dictionary<Type, IObjectPool> _pools = new Dictionary<Type, IObjectPool>();

    private void Awake() {
      foreach (PoolEntry entry in _poolEntries) {
        if (entry.prefab == null) {
          Debug.LogWarning("PoolEntry: prefab undefined");
          continue;
        }

        MonoBehaviour component = entry.prefab.GetComponent<MonoBehaviour>();

        if (component == null) {
          Debug.LogWarning($"Prefab for pool doesn't have a MonoBehaviour: {entry.prefab.name}");
          continue;
        }

        Type compType = component.GetType();
        Type poolType = typeof(ObjectPool<>).MakeGenericType(compType);


        GameObject poolContainer = new GameObject("PoolObject - " + compType.Name);
        poolContainer.transform.SetParent(transform);


        MethodInfo factoryMethod = typeof(ObjectPoolHelpers).GetMethod("CreateFactory")?.MakeGenericMethod(compType);

        object typedFactory = factoryMethod.Invoke(null, new object [] {
          entry.prefab,
          poolContainer.transform
        });


        object poolInstance = Activator.CreateInstance(poolType, typedFactory, entry.initialPoolSize, entry.maxPoolSize, poolContainer.transform);
        _pools[compType] = poolInstance as IObjectPool;
      }
    }


    public T GetObject<T> (Transform targetParent) where T : MonoBehaviour, IPoolable<T> {
      if (_pools.TryGetValue(typeof(T), out IObjectPool pool)) {
        return (pool as IObjectPool<T>).Get(targetParent);
      } else {
        Debug.LogWarning($"Pool for type {typeof(T)} not found.");
        return null;
      }
    }

    public T GetObject<T>() where T : MonoBehaviour, IPoolable<T> {
      return GetObject<T>(null);
    }

  }
}