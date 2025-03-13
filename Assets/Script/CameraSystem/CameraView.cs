using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Script.ObjectPools;

namespace Script.CameraSystem {
  public class CameraView : MonoBehaviour, IPoolable<CameraView> {
    [SerializeField]
    private Camera _camera;
    public IObjectPool<CameraView> Pool { get; set; }

    private Vector3 _currentPosition;
    private float _currentZoom;

    private void Awake() {
      _currentPosition = transform.position;
      _currentZoom = _camera.orthographic ? _camera.orthographicSize : _camera.fieldOfView;
    }

    public void SetStartPosition(Vector3 position) {
      _currentPosition = position;
    }
    public void LerpMove (Vector3 targetPosition, float targetZoom) {
      _currentPosition = Vector3.Lerp(_currentPosition, targetPosition, Time.deltaTime * 10f);
      _currentZoom = Mathf.Lerp(_currentZoom, targetZoom, Time.deltaTime * 10f);

      transform.position = _currentPosition;
      SetZoom(_currentZoom);
    }

    public void SetRotation (float rotationY, float rotationX) {
      transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    private void SetZoom (float zoom) {
      if (_camera.orthographic) {
        _camera.orthographicSize = zoom;
      } else {
        _camera.fieldOfView = zoom;
      }
    }

    public Camera Camera {
      get {
        return _camera;
      }
    }

    public void OnPool() {
      gameObject.SetActive(true);
    }

    public void ReturnToPool() {
      gameObject.SetActive(false);
      Pool?.Release(this);
    }
  }
}