using UnityEngine;
namespace Script.CameraSystem {
  public class CameraController : MonoBehaviour {
    [SerializeField]
    private CameraView _cameraView;
    [SerializeField]
    private CameraModel _cameraModel;

    private CameraPresenter _cameraPresenter;

    private void Awake() {
      _cameraPresenter = new CameraPresenter(_cameraView, _cameraModel);
    }

    private void Update() {
      _cameraPresenter.Update();
    }

    public CameraPresenter CameraPresenter {
      get {
        return _cameraPresenter;
      }
    }
  }
}