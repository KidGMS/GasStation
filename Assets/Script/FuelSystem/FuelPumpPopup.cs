using Script.ObjectPools;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Script.FuelSystem {
  public class FuelPumpPopup : MonoBehaviour, IPoolable<FuelPumpPopup> {
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private FuelPumpPricePanel _pricePanelController;
    [SerializeField]
    private FuelPumpPurchasePanel _purchasePanelController;
    [SerializeField]
    private FuelPumpUpgradePanel _upgradePanelController;

    private FuelPump _currentPump;
    public IObjectPool<FuelPumpPopup> Pool { get; set; }

    public void OnPool() {
      _closeButton.onClick.AddListener(ReturnToPool);
    }

    public void ReturnToPool() {
      _closeButton.onClick.RemoveListener(ReturnToPool);
      gameObject.SetActive(false);
      Pool?.Release(this);
    }

    public void Setup (FuelPump pump) {
      _currentPump = pump;

      if (_upgradePanelController != null) {
        _upgradePanelController.Setup(_currentPump);
      }

      if (_pricePanelController != null) {
        _pricePanelController.Setup(_currentPump);
      }

      if (_purchasePanelController != null) {
        _purchasePanelController.Setup(_currentPump);
      }
    }
  }
}