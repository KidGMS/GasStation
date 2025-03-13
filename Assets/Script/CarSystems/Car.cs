using Script.ObjectPools;
using UnityEngine;

namespace Script.CarSystems {
  public class Car : MonoBehaviour, ICar, IPoolable<Car> {
    [SerializeField]
    private CarStats _carStats;
    [SerializeField]
    private CarVisuals _carVisuals;
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private CarUI _carUI;
    [SerializeField]
    private CarAI _carAI;
    [SerializeField]
    private CarAIData _carAIData;

    private bool _isVisible = true;

    public CarData CarData { get; private set; }
    public CarAI CarAI {
      get {
        return _carAI;
      }
    }
    public CarUI CarUI {
      get {
        return _carUI;
      }
    }

    private void Start() {
      CarData = new CarData(_carStats);
    }

    private void Update() {
      if (!_meshRenderer.isVisible) {
        if (_isVisible) {
          SetVisibility(false);
        }

        return;
      }

      if (!_isVisible) {
        SetVisibility(true);
      }

      ConsumeFuel();
      UpdateUI();
    }

    public void ConsumeFuel() {
      CarData.CurrentFuel -= _carStats.FuelConsumptionRate * Time.deltaTime;
      CarData.CurrentFuel = Mathf.Max(CarData.MaxFuel * 0.05f, CarData.CurrentFuel);
    }

    public void Refuel (float amount) {
      CarData.CurrentFuel += amount;
      CarData.CurrentFuel = Mathf.Min(CarData.CurrentFuel, CarData.MaxFuel);
      UpdateUI();
    }

    public IObjectPool<Car> Pool { get; set; }

    public void OnPool() {
      CarData = new CarData(_carStats);
      ApplyRandomAppearance();
      UpdateUI();
      _carAI.Initialize(this, _carAIData);
      gameObject.SetActive(true);
    }

    public void ReturnToPool() {
      gameObject.SetActive(false);
      Pool?.Release(this);
    }

    private void ApplyRandomAppearance() {
      if (_carVisuals.CarVisualDatas.Count <= 0) {
        return;
      }

      CarVisualData carVisualData = _carVisuals.CarVisualDatas[Random.Range(0, _carVisuals.CarVisualDatas.Count)];
      _meshFilter.mesh = carVisualData.Mesh;
      _meshRenderer.material = carVisualData.Material;
    }

    private void UpdateUI() {
      if (_carUI == null || !_isVisible) {
        return;
      }

      _carUI.UpdateFuel(CarData.CurrentFuel, CarData.MaxFuel);
      _carUI.UpdateSatisfaction(CarData.Satisfaction);
    }

    private void SetVisibility (bool state) {
      _isVisible = state;

      if (_carUI != null) {
        _carUI.SetActiveCanvas(state);
      }
    }
  }
}