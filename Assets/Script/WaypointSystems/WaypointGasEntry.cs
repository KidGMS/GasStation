using Script.BuildingSystems;
using Script.FuelSystem;
using UnityEngine;

namespace Script.WaypointSystems {
  public class WaypointGasEntry : Waypoint {
    [SerializeField]
    private BuildZone _linkedBuildZone;

    public FuelPump FuelPump {
      get {
        if (_linkedBuildZone == null || _linkedBuildZone.Building == null) {
          return null;
        }

        return _linkedBuildZone.Building.GetComponent<FuelPump>();
      }
    }

    public bool HasValidFuelPump() {
      return FuelPump != null;
    }
  }
}