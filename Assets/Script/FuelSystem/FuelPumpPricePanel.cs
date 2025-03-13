using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.FuelSystem {
  public class FuelPumpPricePanel : MonoBehaviour {
    [SerializeField]
    private TMP_Text _fuelPriceText;
    [SerializeField]
    private Button _increasePriceButton;
    [SerializeField]
    private Button _decreasePriceButton;

    private FuelPump _currentPump;

    private void OnDisable() {
      _increasePriceButton.onClick.RemoveListener(OnIncreasePrice);
      _decreasePriceButton.onClick.RemoveListener(OnDecreasePrice);
    }

    public void Setup (FuelPump pump) {
      _currentPump = pump;
      _increasePriceButton.onClick.AddListener(OnIncreasePrice);
      _decreasePriceButton.onClick.AddListener(OnDecreasePrice);
      UpdateUI();
    }

    private void OnIncreasePrice() {
      _currentPump.SetFuelPrice(_currentPump.FuelPrice + 1f);
      UpdateUI();
    }

    private void OnDecreasePrice() {
      _currentPump.SetFuelPrice(_currentPump.FuelPrice - 1f);
      UpdateUI();
    }

    private void UpdateUI() {
      _fuelPriceText.text = $"Price: {_currentPump.FuelPrice}";
    }
  }
}