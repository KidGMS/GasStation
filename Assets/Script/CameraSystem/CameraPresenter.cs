using UnityEngine;

namespace Script.CameraSystem {
  public class CameraPresenter {
    private readonly CameraView _view;
    private readonly CameraModel _model;

    private Vector2 _lastTouchPosition;
    private bool _isDragging;
    private Vector3 _targetPosition;
    private float _targetZoom;
    private readonly Vector3 _startPosition;

    public CameraPresenter (CameraView view, CameraModel model) {
      _view = view;
      _model = model;
      _startPosition = _view.transform.position; 
      _targetPosition = _startPosition;
      _targetZoom = GetInitialZoom();
      _view.SetRotation(-45f, 30f);
    }

    public void Update() {
      HandleKeyboardMouseInput();
      HandleMouseInput();
      HandleTouchControls();
      ApplyMovement();
    }

    private float GetInitialZoom() {
      return _view.Camera.orthographic ? _view.Camera.orthographicSize : _view.Camera.fieldOfView;
    }

    private void HandleKeyboardMouseInput() {
      Vector3 moveDirection = GetKeyboardMovement();

      if (moveDirection != Vector3.zero) {
        _targetPosition = Vector3.Lerp(_targetPosition, _targetPosition + moveDirection * _model.MoveSpeed, Time.deltaTime * 5f);
      }
    }

    private Vector3 GetKeyboardMovement() {
      Vector3 moveDirection = Vector3.zero;
      Vector3 flatForward = Vector3.ProjectOnPlane(_view.transform.forward, Vector3.up).normalized;
      Vector3 flatRight = Vector3.ProjectOnPlane(_view.transform.right, Vector3.up).normalized;

      if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
        moveDirection += flatForward;
      }

      if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
        moveDirection -= flatForward;
      }

      if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
        moveDirection -= flatRight;
      }

      if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
        moveDirection += flatRight;
      }

      return moveDirection.normalized;
    }

    private void HandleMouseInput() {
      if (Input.GetMouseButtonDown(2)) {
        StartDragging();
      }

      if (Input.GetMouseButton(2) && _isDragging) {
        DragCamera();
      }

      if (Input.GetMouseButtonUp(2)) {
        StopDragging();
      }

      HandleMouseZoom();
    }

    private void StartDragging() {
      _lastTouchPosition = Input.mousePosition;
      _isDragging = true;
    }

    private void DragCamera() {
      Vector2 delta = (Vector2)Input.mousePosition - _lastTouchPosition;
      _lastTouchPosition = Input.mousePosition;

      Vector3 flatRight = Vector3.ProjectOnPlane(_view.transform.right, Vector3.up).normalized;
      Vector3 flatForward = Vector3.ProjectOnPlane(_view.transform.forward, Vector3.up).normalized;

      _targetPosition += (-flatRight * delta.x + -flatForward * delta.y) * _model.MoveSpeed * 0.02f;
    }

    private void StopDragging() {
      _isDragging = false;
    }

    private void HandleMouseZoom() {
      float scroll = Input.GetAxis("Mouse ScrollWheel");

      if (Mathf.Abs(scroll) > 0.01f) {
        _targetZoom -= scroll * _model.ZoomSpeed;
      }
    }

    private void HandleTouchControls() {
      if (Input.touchCount == 1) {
        HandleSingleTouch();
      }

      if (Input.touchCount == 2) {
        HandlePinchZoom();
      }
    }

    private void HandleSingleTouch() {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Began) {
        StartDragging();
      }

      if (touch.phase == TouchPhase.Moved && _isDragging) {
        Vector2 delta = touch.deltaPosition * _model.MoveSpeed * 0.02f;

        Vector3 flatRight = Vector3.ProjectOnPlane(_view.transform.right, Vector3.up).normalized;
        Vector3 flatForward = Vector3.ProjectOnPlane(_view.transform.forward, Vector3.up).normalized;

        _targetPosition += -flatRight * delta.x + -flatForward * delta.y;
      }

      if (touch.phase == TouchPhase.Ended) {
        StopDragging();
      }
    }

    private void HandlePinchZoom() {
      Touch touch0 = Input.GetTouch(0);
      Touch touch1 = Input.GetTouch(1);

      float prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
      float currentDistance = (touch0.position - touch1.position).magnitude;
      float delta = prevDistance - currentDistance;

      _targetZoom += delta * _model.ZoomSpeed * 0.02f;
    }

    private void ApplyMovement() {
      _targetPosition = ClampPosition(_targetPosition);
      _targetZoom = Mathf.Clamp(_targetZoom, _model.MinZoom, _model.MaxZoom);
      _view.LerpMove(_targetPosition, _targetZoom);
    }

    private Vector3 ClampPosition (Vector3 position) {
      return new Vector3(Mathf.Clamp(position.x, _startPosition.x + _model.XLimits.x, _startPosition.x + _model.XLimits.y), _view.transform.position.y,
        Mathf.Clamp(position.z, _startPosition.z + _model.ZLimits.x, _startPosition.z + _model.ZLimits.y));
    }
  }
}