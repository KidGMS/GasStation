using UnityEngine;
using UnityEngine.Serialization;

namespace Script.EconomySystems {
  [CreateAssetMenu(fileName = "EconomySettings", menuName = "Settings/Economy Settings")]
  public class EconomySettings : ScriptableObject {
    public int DefaultBalance = 1000;
  }
}