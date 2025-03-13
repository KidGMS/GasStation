using System;

namespace Script.FuelSystem {
  [Serializable]
  public class FuelPumpData {
    public int Level;
    public float FuelAmount;
    public float FuelPrice;

    public FuelPumpData() {}

    public FuelPumpData (int level, float fuelAmount, float fuelPrice) {
      Level = level;
      FuelAmount = fuelAmount;
      FuelPrice = fuelPrice;
    }
  }
}