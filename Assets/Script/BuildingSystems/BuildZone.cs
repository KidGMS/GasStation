using System.Collections.Generic;
using Script.Construction;
using Script.DI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.BuildingSystems {
  public class BuildZone : MonoBehaviour {
    [SerializeField]
    private Transform _buildPosition;
    [SerializeField]
    private List<GameObject> _animationVisual;
    [SerializeField]
    private Building _building;
    public Building Building {
      get {
        return _building;
      }
      set {
        _building = value;
        MarkAsBuilt();
      }
    }
    public Vector3 BuildPosition {
      get {
        return _buildPosition.position;
      }
    }
    public bool IsOccupied {
      get;
      private set;
    }

    private void OnMouseDown() {
      if (IsPointerOverUIObject()) {
        return;
      }

      if (IsOccupied) {
        return;
      }

      DiManager.Instance.Container.Resolve<BuildManager>().OpenBuildMenu(this);
    }

    public void MarkAsBuilt() {
      IsOccupied = true;

      foreach (GameObject visual in _animationVisual) {
        visual.gameObject.SetActive(false);
      }
    }

    public void MarkAsEmpty() {
      IsOccupied = false;
    }

    private bool IsPointerOverUIObject() {
#if UNITY_EDITOR || UNITY_STANDALONE
      return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0) {
                PointerEventData eventData = new PointerEventData(EventSystem.current) {
                    position = Input.GetTouch(0).position
                };
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                return results.Count > 0;
            }
            return false;
#else
            return false;
#endif
    }
  }
}