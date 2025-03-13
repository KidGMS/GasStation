using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.FuelSystem {
  [Serializable]
  public class FuelPumpLevelData {
    public int Level;
    public List<GameObject> Objects;
  }
}