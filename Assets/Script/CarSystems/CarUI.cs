using UnityEngine;
using UnityEngine.UI;

namespace Script.CarSystems {
  public class CarUI : MonoBehaviour {
    [SerializeField]
    private CarUISettings _uiSettings;
    [SerializeField]
    private Image _fuelBar;
    [SerializeField]
    private Image _satisfactionBar;
    [SerializeField]
    private Canvas _canvas;
    private bool _isActive = true;

    private Camera _mainCamera;

    private void Update() {
      if (!_isActive) {
        return;
      }

      UpdateScaleByCamera();
      LookAtCamera();
    }

    public void SetupCamera (Camera camera) {
      _canvas.worldCamera = camera;
      _mainCamera = camera;
    }

    private void LookAtCamera() {
      if (_mainCamera == null) {
        return;
      }

      Vector3 cameraPosition = _mainCamera.transform.position;
      _canvas.transform.LookAt(cameraPosition);
      _canvas.transform.Rotate(0, -180f, 0);
    }

    public void UpdateFuel (float current, float max) {
      float normalizedValue = current / max;
      SetBarScale(_fuelBar, normalizedValue);
    }

    public void UpdateSatisfaction (float value) {
      float normalizedValue = value / 100f;
      SetBarScale(_satisfactionBar, normalizedValue);
    }

    private void SetBarScale (Image bar, float value) {
      if (bar == null) {
        return;
      }

      Vector3 scale = bar.transform.localScale;
      scale.x = Mathf.Clamp(value * _uiSettings.MaxScaleX, 0.01f, _uiSettings.MaxScaleX);
      bar.transform.localScale = scale;
      bar.pixelsPerUnitMultiplier = Mathf.Lerp(1f, _uiSettings.MaxPPU, value);
    }

    private void UpdateScaleByCamera() {
      if (_mainCamera == null) {
        return;
      }

      float currentFOV = _mainCamera.fieldOfView;
      float normalizedFOV = Mathf.InverseLerp(_uiSettings.CameraModel.MinZoom, _uiSettings.CameraModel.MaxZoom, currentFOV);
      float newScale = Mathf.Lerp(_uiSettings.MinScale, _uiSettings.MaxScale, normalizedFOV);

      _canvas.transform.localScale = Vector3.one * newScale;
    }

    public void SetActiveCanvas (bool state) {
      _isActive = state;
      _canvas.enabled = state;
    }
  }
}