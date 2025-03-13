using System;
using UnityEngine;
namespace Script.ObjectPools {
  [Serializable]
  public class PoolEntry {
    public GameObject prefab;
    public int initialPoolSize = 10;
    public int maxPoolSize = 20;
  }
}