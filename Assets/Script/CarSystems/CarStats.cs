using UnityEngine;

namespace Script.CarSystems {
  [CreateAssetMenu(fileName = "CarStats", menuName = "SO/Settings/Car Stats")]
  public class CarStats : ScriptableObject {
    public float MinFuelCapacity = 30f;
    public float MaxFuelCapacity = 100f;
    public float MinSatisfaction = 50f;
    public float MaxSatisfaction = 100f;
    public float FuelConsumptionRate = 0.05f;

    public float MinSpeed = 5f;
    public float MaxSpeed = 10f;
  }
}