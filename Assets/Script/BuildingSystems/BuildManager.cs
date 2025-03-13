using UnityEngine;
using Script.BuildingSystems;
using Script.Construction;
using Script.DI;
using Script.ObjectPools;
using Script.SaveSystem;
using Script.UI;
using Script.EconomySystems;

namespace Script.BuildingSystems {
  public class BuildManager : MonoBehaviour, IBuildHandler {
    [SerializeField]
    private Transform _gameCanvas;

    private PoolManager _poolManager;
    private BuildZone _currentZone;
    private EconomyPresenter _economy;
    private BuildSettings _buildSettings;
    private BuildingSystemPopup _buildingSystemPopup;

    private void Start() {
      _poolManager = DiManager.Instance.Container.Resolve<PoolManager>();
      _economy = DiManager.Instance.Container.Resolve<EconomyManager>().GetPresenter();
      _buildSettings = DiManager.Instance.Container.Resolve<BuildSettings>();

      LoadBuildings();
    }

    public void OpenBuildMenu (BuildZone zone) {
      _currentZone = zone;
      _buildingSystemPopup = DiManager.Instance.Container.Resolve<PoolManager>().GetObject<BuildingSystemPopup>(_gameCanvas);
    }

    public void ConstructBuilding() {
      if (_currentZone == null || _currentZone.IsOccupied) {
        return;
      }

      if (!_economy.SpendMoney(_buildSettings.BuildingCost)) {

        _buildingSystemPopup.ReturnToPool();
        return;
      }

      Building building = _poolManager.GetObject<Building>(_currentZone.transform);

      if (building == null) {
        return;
      }

      building.PlaceAt(_currentZone);
      building.Initialize(Random.Range(1, 5), "Player");
      _currentZone.Building = building;
      _currentZone.MarkAsBuilt();
      SaveConstruction(building);
      _buildingSystemPopup.ReturnToPool();
    }

    private void SaveConstruction (IBuildableObject building) {
      ConstructionData data = DiManager.Instance.Container.Resolve<SaveManager>().Load<ConstructionData>(SaveConstants.GAME_STATE) ?? new ConstructionData();

      data.Buildings.Add(building.GetSaveData());
      DiManager.Instance.Container.Resolve<SaveManager>().Save(SaveConstants.GAME_STATE, data);
    }

    private void LoadBuildings() {
      if (!DiManager.Instance.Container.Resolve<SaveManager>().Exists(SaveConstants.GAME_STATE)) {
        return;
      }

      ConstructionData data = DiManager.Instance.Container.Resolve<SaveManager>().Load<ConstructionData>(SaveConstants.GAME_STATE);

      foreach (ConstructionEntry entry in data.Buildings) {
        Building building = _poolManager.GetObject<Building>();

        if (building == null) {
          continue;
        }

        BuildZone zone = FindZoneAt(entry.Position);

        if (zone == null) {
          continue;
        }

        building.PlaceAt(zone);
        building.transform.parent = zone.transform;
        building.Initialize(entry.Level, entry.Owner);
        zone.Building = building;
        zone.MarkAsBuilt();
      }
    }

    private BuildZone FindZoneAt (Vector3 position) {
      foreach (BuildZone zone in FindObjectsOfType<BuildZone>()) {
        if (Vector3.Distance(zone.transform.position, position) < 0.1f) {
          return zone;
        }
      }

      return null;
    }
  }
}