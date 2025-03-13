using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.FuelSystem {
  public class FuelPumpUpgradePanel : MonoBehaviour {
    [SerializeField]
    private TMP_Text _pumpLevelText;
    [SerializeField]
    private TMP_Text _upgradeCostText;
    [SerializeField]
    private TMP_Text _currentCapacityText;
    [SerializeField]
    private TMP_Text _currentRefuelSpeedText;
    [SerializeField]
    private TMP_Text _currentRestockSpeedText;
    [SerializeField]
    private TMP_Text _currentQueueSizeText;
    [SerializeField]
    private TMP_Text _nextCapacityText;
    [SerializeField]
    private TMP_Text _nextRefuelSpeedText;
    [SerializeField]
    private TMP_Text _nextRestockSpeedText;
    [SerializeField]
    private TMP_Text _nextQueueSizeText;
    [SerializeField]
    private Button _upgradeButton;

    private FuelPump _currentPump;

    private void OnDisable() {
      _upgradeButton.onClick.RemoveListener(OnUpgrade);
    }

    public void Setup (FuelPump pump) {
      _currentPump = pump;
      _upgradeButton.onClick.AddListener(OnUpgrade);
      UpdateUI();
    }

    private void OnUpgrade() {
      _currentPump.UpgradePump();
      UpdateUI();
    }

    private void UpdateUI() {
      _pumpLevelText.text = $"Level: {_currentPump.Level}";
      _upgradeCostText.text = $"Upgrade Cost: {GetUpgradeCost()}";
      UpdateCurrentParams();
      UpdateNextParams();
    }

    private int GetUpgradeCost() {
      return (int)_currentPump.Settings.DefaultPrice * 1500 * _currentPump.Level;
    }

    private void UpdateCurrentParams() {
      if (_currentPump.Settings == null) {
        ClearCurrentParams();
        return;
      }

      int currentIndex = _currentPump.Level - 1;

      if (currentIndex >= 0 && currentIndex < _currentPump.Settings.LevelUpgrades.Count) {
        FuelPumpUpgradeData data = _currentPump.Settings.LevelUpgrades[currentIndex];
        _currentCapacityText.text = $"Capacity: {data.MaxFuelCapacity}";
        _currentRefuelSpeedText.text = $"Refuel Speed: {data.RefuelSpeed}";
        _currentRestockSpeedText.text = $"Restock Speed: {data.FuelRestockSpeed}";
        _currentQueueSizeText.text = $"Queue Size: {data.MaxQueueSize}";
      } else {
        _currentCapacityText.text = "N/A";
        _currentRefuelSpeedText.text = "N/A";
        _currentRestockSpeedText.text = "N/A";
        _currentQueueSizeText.text = "N/A";
      }
    }

    private void ClearCurrentParams() {
      _currentCapacityText.text = "";
      _currentRefuelSpeedText.text = "";
      _currentRestockSpeedText.text = "";
      _currentQueueSizeText.text = "";
    }

    private void ClearNextParams() {
      _nextCapacityText.text = "";
      _nextRefuelSpeedText.text = "";
      _nextRestockSpeedText.text = "";
      _nextQueueSizeText.text = "";
    }

    private void UpdateNextParams() {
      if (_currentPump.Settings == null) {
        ClearNextParams();
        return;
      }

      int nextIndex = _currentPump.Level;

      if (nextIndex < _currentPump.Settings.LevelUpgrades.Count) {
        FuelPumpUpgradeData data = _currentPump.Settings.LevelUpgrades[nextIndex];
        _nextCapacityText.text = $"Capacity: {data.MaxFuelCapacity}";
        _nextRefuelSpeedText.text = $"Refuel Speed: {data.RefuelSpeed}";
        _nextRestockSpeedText.text = $"Restock Speed: {data.FuelRestockSpeed}";
        _nextQueueSizeText.text = $"Queue Size: {data.MaxQueueSize}";
      } else {
        _nextCapacityText.text = "Max Level Reached";
        _nextRefuelSpeedText.text = "";
        _nextRestockSpeedText.text = "";
        _nextQueueSizeText.text = "";
      }
    }
  }
}