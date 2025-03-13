using UnityEngine;
using UnityEngine.Serialization;

namespace Script.CarSystems {
  [CreateAssetMenu(fileName = "CarAIData", menuName = "SO/Settings/Car AI Data")]
  public class CarAIData : ScriptableObject {
    public float StopDistance = 2f;
    public float DetectionRange = 5f;
    public float StopDistanceMultiplier = 5f;
    public float BrakingLerpSpeed = 4f;      
    public float AccelerationLerpSpeed = 2f; 
    public float CollisionBoxWidth = 2f;
    public float CollisionBoxHeight = 1f;
    public float TurnSpeed = 5f;
  }
}