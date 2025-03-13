using UnityEngine;

namespace Script.CarSystems {
  [CreateAssetMenu(fileName = "CarSystemData", menuName = "SO/Settings/Car System Data")]
  public class CarSystemData : ScriptableObject {
    public int MaxCars = 10;
    public float StartTime = 1f;
    public float RepeatTime = 3f;
  }
}