using UnityEngine;

namespace Script.CameraSystem {
  [CreateAssetMenu(fileName = "CameraSettings", menuName = "SO/Camera/Camera Settings")]
  public class CameraModel : ScriptableObject {
    [Header("Movement")]
    public float MoveSpeed = 10f;

    [Header("Zoom")]
    public float ZoomSpeed = 10f;
    public float MinZoom = 5f;
    public float MaxZoom = 30f;

    [Header("BORDER")]
    public Vector2 XLimits = new Vector2(-50f, 50f);
    public Vector2 ZLimits = new Vector2(-50f, 50f);
  }
}