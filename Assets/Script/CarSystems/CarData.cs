using UnityEngine;

namespace Script.CarSystems {
  [System.Serializable]
  public class CarData {
    public float MaxFuel;
    public float CurrentFuel;
    public float Satisfaction;
    public float Speed;

    public CarData(CarStats stats) {
      MaxFuel = Random.Range(stats.MinFuelCapacity, stats.MaxFuelCapacity);
      CurrentFuel = MaxFuel * Random.Range(0.2f, 1f);
      Satisfaction = Random.Range(stats.MinSatisfaction, stats.MaxSatisfaction);
      Speed = Random.Range(stats.MinSpeed, stats.MaxSpeed);
    }
  }
}