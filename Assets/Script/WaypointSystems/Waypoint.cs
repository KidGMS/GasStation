using System.Collections.Generic;
using UnityEngine;

namespace Script.WaypointSystems {
  public class Waypoint : MonoBehaviour {
    [SerializeField]
    private List<Waypoint> _nextWaypoints = new List<Waypoint>();
    [SerializeField]
    private WaypointType _waypointType;

    public List<Waypoint> NextWaypoints {
      get {
        return _nextWaypoints;
      }
    }
    public WaypointType Type {
      get {
        return _waypointType;
      }
    }

    private void OnDrawGizmos() {
      Gizmos.color = GetGizmoColor();
      Gizmos.DrawSphere(transform.position, 1.5f);

      if (_nextWaypoints == null || _nextWaypoints.Count == 0) {
        return;
      }

      Gizmos.color = Color.yellow;

      foreach (Waypoint waypoint in _nextWaypoints) {
        if (waypoint != null) {
          Gizmos.DrawLine(transform.position, waypoint.transform.position);
        }
      }
    }
    
    private Color GetGizmoColor() {
      return _waypointType switch {
        WaypointType.Spawn => Color.white,
        WaypointType.Road => Color.green,
        WaypointType.Entry => Color.blue,
        WaypointType.GasRoad => new Color(0.3f, 0.7f, 1f),
        WaypointType.GasEntry => Color.cyan,
        WaypointType.GasAltEntry => new Color(0f, 1f, 1f),
        WaypointType.GasExit => Color.yellow,
        WaypointType.Exit => Color.magenta,
        WaypointType.Despawn => Color.red,
        _ => Color.white
      };
    }
  }

  public enum WaypointType {
    Spawn,      
    Road,       
    Entry,      
    GasRoad,    
    GasEntry,   
    GasAltEntry,
    GasExit,    
    Exit,       
    Despawn,    
    GasRefueling
  }
}