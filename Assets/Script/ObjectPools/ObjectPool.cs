using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.ObjectPools {
  public class ObjectPool<T> : IObjectPool<T>, IObjectPool where T : MonoBehaviour, IPoolable<T> {
    private readonly Stack<T> _pool;
    private readonly Func<T> _factory;
    private readonly int _maxPoolSize;
    private readonly Transform _containerTransform;
    private int _currentCount;

    public ObjectPool (Func<T> factory, int initialCapacity, int maxPoolSize, Transform containerTransform) {
      if (factory == null) {
        throw new ArgumentNullException(nameof(factory));
      }

      if (initialCapacity < 0 || maxPoolSize < initialCapacity) {
        throw new ArgumentException("maxPoolSize must be >= initialCapacity");
      }

      if (containerTransform == null) {
        throw new ArgumentNullException(nameof(containerTransform));
      }

      _factory = factory;
      _maxPoolSize = maxPoolSize;
      _containerTransform = containerTransform;
      _pool = new Stack<T>(maxPoolSize);

      for (int i = 0; i < initialCapacity; i++) {
        T instance = CreateInstance();
        _pool.Push(instance);
      }

      _currentCount = initialCapacity;
    }

    private T CreateInstance() {
      T instance = _factory();
      instance.gameObject.SetActive(false);
      instance.Pool = this;
      instance.transform.SetParent(_containerTransform, false);
      return instance;
    }

    public T Get() {
      return Get(null);
    }

    public T Get (Transform targetParent) {
      T instance;

      if (_pool.Count > 0) {
        instance = _pool.Pop();
      } else if (_currentCount < _maxPoolSize) {
        _currentCount++;
        instance = CreateInstance();
      } else {
        Debug.LogWarning("Max pool size reached.");
        return null;
      }

      instance.gameObject.SetActive(true);
      instance.OnPool();

      if (targetParent != null) {
        instance.transform.SetParent(targetParent, false);
      }

      return instance;
    }

    public void Release (T instance) {
      instance.transform.SetParent(_containerTransform, false);
      instance.gameObject.SetActive(false);
      _pool.Push(instance);
    }
  }
}