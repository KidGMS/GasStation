using Script.CarSystems;

namespace Script.FuelSystem {
  public interface IFuelPump {
    bool CanRefuel (Car car);

    void EnqueueCar (Car car);

    void UpgradePump();

    void SetFuelPrice (float price);

    void PurchaseFuel (float amount);
  }
}