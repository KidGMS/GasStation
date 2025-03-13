using System.Collections.Generic;
using UnityEngine;

namespace Script.FuelSystem {
  [CreateAssetMenu(fileName = "FuelPumpSettings", menuName = "SO/Settings/FuelPump Settings")]
  public class FuelPumpSettings : ScriptableObject {
    public int MaxLevel;
    public float DefaultPrice;
    public List<FuelPumpUpgradeData> LevelUpgrades;

    public float GetMaxFuelCapacity (int level) {
      int index = level - 1;

      if (index < 0 || index >= LevelUpgrades.Count) {
        return LevelUpgrades.Count > 0 ? LevelUpgrades[LevelUpgrades.Count - 1].MaxFuelCapacity : 0f;
      }

      return LevelUpgrades[index].MaxFuelCapacity;
    }

    public float GetRefuelSpeed (int level) {
      int index = level - 1;

      if (index < 0 || index >= LevelUpgrades.Count) {
        return LevelUpgrades.Count > 0 ? LevelUpgrades[LevelUpgrades.Count - 1].RefuelSpeed : 0f;
      }

      return LevelUpgrades[index].RefuelSpeed;
    }

    public float GetFuelRestockSpeed (int level) {
      int index = level - 1;

      if (index < 0 || index >= LevelUpgrades.Count) {
        return LevelUpgrades.Count > 0 ? LevelUpgrades[LevelUpgrades.Count - 1].FuelRestockSpeed : 0f;
      }

      return LevelUpgrades[index].FuelRestockSpeed;
    }

    public int GetMaxQueueSize (int level) {
      int index = level - 1;

      if (index < 0 || index >= LevelUpgrades.Count) {
        return LevelUpgrades.Count > 0 ? LevelUpgrades[LevelUpgrades.Count - 1].MaxQueueSize : 1;
      }

      return LevelUpgrades[index].MaxQueueSize;
    }
  }
}