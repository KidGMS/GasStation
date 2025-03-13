using Script.DI;
using Script.EconomySystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.FuelSystem {
  public class FuelPumpPurchasePanel : MonoBehaviour {
    [SerializeField]
    private Slider _purchaseSlider;
    [SerializeField]
    private TMP_Text _purchaseAmountText;
    [SerializeField]
    private TMP_Text _purchaseCostText;
    [SerializeField]
    private TMP_Text _currentFuelText;
    [SerializeField]
    private Button _purchaseButton;

    private FuelPump _currentPump;

    private void OnDisable() {
      _purchaseSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
      _purchaseButton.onClick.RemoveListener(OnPurchaseFuel);
    }

    public void Setup (FuelPump pump) {
      _currentPump = pump;
      _purchaseSlider.onValueChanged.AddListener(OnSliderValueChanged);
      _purchaseButton.onClick.AddListener(OnPurchaseFuel);
      SetSliderLimits();
      UpdateUI();
    }

    private void SetSliderLimits() {
      float maxPurchase = _currentPump.GetMaxFuelCapacity() - _currentPump.CurrentFuelAmount;
      _purchaseSlider.minValue = 0f;
      _purchaseSlider.maxValue = maxPurchase > 0 ? maxPurchase : 0;

      if (_purchaseSlider.value > _purchaseSlider.maxValue) {
        _purchaseSlider.value = _purchaseSlider.maxValue;
      }
    }

    private void OnSliderValueChanged (float value) {
      UpdateUI();
    }

    private void OnPurchaseFuel() {
      float amount = _purchaseSlider.value;
      _currentPump.PurchaseFuel(amount);
      EconomyManager economyManager = DiManager.Instance.Container.Resolve<EconomyManager>();
      float cost = GetDiscountedCost(amount);
      economyManager.GetPresenter().SpendMoney((int)cost);
      SetSliderLimits();
      UpdateUI();
    }

    private float GetDiscountedCost (float amount) {
      float baseCost = amount * _currentPump.BaseFuelPrice;
      float discount = Mathf.Clamp(amount / 100f, 0f, 0.2f);
      return baseCost * (1 - discount);
    }

    private void UpdateUI() {
      float amount = _purchaseSlider.value;
      _purchaseAmountText.text = $"{amount:F1} gallons";
      float cost = GetDiscountedCost(amount);
      _purchaseCostText.text = $"Cost: {cost:F0}";
      _currentFuelText.text = $"Current Fuel: {_currentPump.CurrentFuelAmount} gallons";
    }
  }
}