using UnityEngine;

namespace Script.ObjectPools {
  public interface IObjectPool<T> {
    T Get();

    T Get (Transform targetParent);

    void Release (T instance);
  }

  public interface IObjectPool {}

}