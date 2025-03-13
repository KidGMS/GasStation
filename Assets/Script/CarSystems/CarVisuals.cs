using System.Collections.Generic;
using UnityEngine;

namespace Script.CarSystems {
  [CreateAssetMenu(fileName = "CarVisuals", menuName = "SO/Settings/Car Visuals")]
  public class CarVisuals : ScriptableObject {
    public List<CarVisualData> CarVisualDatas;
  }
}