using Script.DI;
using Script.ObjectPools;
using Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Script.FuelSystem {
  public class FuelPumpPopupOpener : MonoBehaviour {
    private FuelPump _fuelPump;

    private void Awake() {
      _fuelPump = GetComponent<FuelPump>();
    }

    private void OnMouseDown() {
      if (IsPointerOverUIObject()) {
        return;
      }

      if (_fuelPump == null) {
        return;
      }

      GameObject uiParent = GameObject.FindGameObjectWithTag("MainCanvas");

      if (uiParent == null) {
        return;
      }

      PoolManager poolManager = DiManager.Instance.Container.Resolve<PoolManager>();
      FuelPumpPopup popup = poolManager.GetObject<FuelPumpPopup>(uiParent.transform);
      popup.Setup(_fuelPump);
    }

    private bool IsPointerOverUIObject() {
#if UNITY_EDITOR || UNITY_STANDALONE
      return EventSystem.current.IsPointerOverGameObject();
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) {
            PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.GetTouch(0).position };
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