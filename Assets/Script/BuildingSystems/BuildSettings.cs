using UnityEngine;

namespace Script.BuildingSystems {
  [CreateAssetMenu(fileName = "BuildSettings", menuName = "SO/Settings/Build Settings")]
  public class BuildSettings : ScriptableObject {
    public int BuildingCost = 500;
  }
}