using System;
using UnityEngine;
namespace Script.ObjectPools {
  public static class ObjectPoolHelpers {
    public static Func<T> CreateFactory<T> (GameObject prefab, Transform parent) where T : MonoBehaviour {
      return () => {
        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        go.SetActive(false);
        return go.GetComponent<T>();
      };
    }
  }
}