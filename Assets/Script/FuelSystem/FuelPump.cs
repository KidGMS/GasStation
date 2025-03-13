using System.Collections;
using System.Collections.Generic;
using Script.CarSystems;
using Script.DI;
using Script.EconomySystems;
using Script.SaveSystem;
using Script.Stat;
using Script.Statistics;
using UnityEngine;

namespace Script.FuelSystem {
  public class FuelPump : MonoBehaviour, IFuelPump {
    [SerializeField]
    private List<FuelPumpLevelData> _levelData;
    [SerializeField]
    private FuelPumpSettings _settings;

    private readonly Queue<Car> _queue = new Queue<Car>();
    private bool _isRefueling;
    private SaveManager _saveManager;

    public float FuelPrice { get; private set; }
    public float BaseFuelPrice {
      get {
        return _settings?.DefaultPrice ?? 1f;
      }
    }
    public bool IsOutOfFuel {
      get {
        return CurrentFuelAmount <= 0;
      }
    }

    public int Level { get; private set; }
    public float CurrentFuelAmount { get; private set; }
    public FuelPumpSettings Settings {
      get {
        return _settings;
      }
    }

    private string SaveKey {
      get {
        return transform.parent != null ? transform.parent.name : gameObject.name;
      }
    }

    private void Start() {
      _saveManager = DiManager.Instance.Container.Resolve<SaveManager>();

      if (_settings == null) {
        return;
      }

      LoadPumpData();
      UpdateVisual();
    }

    public bool CanRefuel (Car car) {
      return _queue.Count < _settings.GetMaxQueueSize(Level) && !IsOutOfFuel;
    }

    public void EnqueueCar (Car car) {
      if (!_queue.Contains(car)) {
        _queue.Enqueue(car);
        TryStartRefueling();
      }
    }

    public void UpgradePump() {
      if (Level < _settings.MaxLevel) {
        Level++;
        SavePumpData();
        UpdateVisual();
      }
    }

    public void SetFuelPrice (float price) {
      FuelPrice = Mathf.Clamp(price, BaseFuelPrice, BaseFuelPrice * 4f);
      SavePumpData();
    }

    public void PurchaseFuel (float amount) {
      if (amount <= 0) {
        return;
      }

      CurrentFuelAmount += amount;
      SavePumpData();
    }

    public float GetMaxFuelCapacity() {
      return _settings.GetMaxFuelCapacity(Level);
    }

    private void TryStartRefueling() {
      if (_queue.Count > 0 && !_isRefueling && !IsOutOfFuel) {
        StartCoroutine(RefuelCar(_queue.Peek()));
      }
    }

    private IEnumerator RefuelCar (Car car) {
      _isRefueling = true;
      float fuelNeeded = car.CarData.MaxFuel - car.CarData.CurrentFuel;
      float percentage = Random.Range(0.2f, 1f);
      float fuelToGive = Mathf.Min(fuelNeeded * percentage, CurrentFuelAmount);
      float timeToRefuel = fuelToGive / _settings.GetRefuelSpeed(Level);

      float elapsed = 0f;
      float refuelRate = fuelToGive / timeToRefuel;

      while (elapsed < timeToRefuel) {
        float deltaFuel = refuelRate * Time.deltaTime;
        car.Refuel(deltaFuel);
        elapsed += Time.deltaTime;
        yield return null;
      }

      CurrentFuelAmount -= fuelToGive;
      _isRefueling = false;
      SavePumpData();

      if (_queue.Count > 0 && _queue.Peek() == car) {
        _queue.Dequeue();
      }

      TryStartRefueling();

      EconomyManager economyManager = DiManager.Instance.Container.Resolve<EconomyManager>();
      float revenue = fuelToGive * FuelPrice;
      economyManager.GetPresenter().AddMoney((int)revenue);

      IStatisticsManager statsManager = DiManager.Instance.Container.Resolve<IStatisticsManager>();
      statsManager.AddFuelingData(revenue, fuelToGive, false, true);
      car.CarAI.OnRefueled();
    }

    public bool IsCarEnqueued (Car car) {
      return _queue.Contains(car);
    }

    public int GetQueueIndex (Car car) {
      int index = 0;

      foreach (Car c in _queue) {
        if (c == car) {
          return index;
        }

        index++;
      }

      return -1;
    }

    private void UpdateVisual() {
      foreach (FuelPumpLevelData levelData in _levelData) {
        bool isActiveLevel = levelData.Level == Level;

        foreach (GameObject obj in levelData.Objects) {
          obj.SetActive(isActiveLevel);
        }
      }
    }

    private void LoadPumpData() {
      if (_saveManager == null || _settings == null) {
        return;
      }

      if (_saveManager.Exists(SaveKey)) {
        FuelPumpData data = _saveManager.Load<FuelPumpData>(SaveKey);

        if (data != null && data.FuelAmount > 0) {
          Level = data.Level;
          CurrentFuelAmount = data.FuelAmount;
          FuelPrice = data.FuelPrice;
          return;
        }
      }

      Level = 1;
      CurrentFuelAmount = _settings.GetMaxFuelCapacity(Level);
      FuelPrice = _settings.DefaultPrice;
      SavePumpData();
    }

    private void SavePumpData() {
      _saveManager?.Save(SaveKey, new FuelPumpData(Level, CurrentFuelAmount, FuelPrice));
    }
  }
}