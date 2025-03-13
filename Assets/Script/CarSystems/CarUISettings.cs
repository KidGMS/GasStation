using Script.CameraSystem;
using UnityEditor;
using UnityEngine;

namespace Script.CarSystems {
  [CreateAssetMenu(fileName = "CarUISettings", menuName = "SO/Settings/Car UI Settings")]
  public class CarUISettings : ScriptableObject {
    public float MinScale = 0.5f;
    public float MaxScale = 1.5f;
    public CameraModel CameraModel;
    public float MaxScaleX = 1f;
    public float MaxPPU = 25f;
  }
}