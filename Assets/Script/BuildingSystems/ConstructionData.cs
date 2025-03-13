using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Construction {
  [Serializable]
  public class ConstructionData {
    public List<ConstructionEntry> Buildings = new List<ConstructionEntry>();
  }

  [Serializable]
  public class ConstructionEntry {
    public Vector3 Position;
    public Transform Transform;
    public int PrefabID;
    public int Level;
    public string Owner;
  }
}