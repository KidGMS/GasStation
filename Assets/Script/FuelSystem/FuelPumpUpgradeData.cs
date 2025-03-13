using System;

namespace Script.FuelSystem {
  [Serializable]
  public class FuelPumpUpgradeData {
    public int Level;
    public float MaxFuelCapacity;
    public float RefuelSpeed;
    public float FuelRestockSpeed;
    public int MaxQueueSize;
  }
}