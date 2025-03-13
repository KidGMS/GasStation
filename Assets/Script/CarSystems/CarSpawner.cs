using System.Collections.Generic;
using Script.DI;
using Script.ObjectPools;
using Script.WaypointSystems;
using UnityEngine;

namespace Script.CarSystems {
  public class CarSpawner : MonoBehaviour {
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private CarSystemData _carSystemData;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Waypoint _startWaypoint;
    
    private IObjectPool<Car> _carPool;
    private PoolManager _poolManager;
    private readonly List<Car> _spawnedCars = new List<Car>();

    private void Start() {
      _poolManager = DiManager.Instance.Container.Resolve<PoolManager>();
      InvokeRepeating(nameof(SpawnCar), _carSystemData.StartTime, _carSystemData.RepeatTime);
    }

    private void SpawnCar() {
      Car car = _poolManager.GetObject<Car>(_spawnPoint);

      if (car == null) {
        return;
      }

      car.transform.localPosition = Vector3.zero;
      car.transform.localEulerAngles = Vector3.zero;
      car.CarUI.SetupCamera(_camera);

      CarAI carAI = car.CarAI;
      carAI.SetStartWaypoint(_startWaypoint);
      _spawnedCars.Add(car);
    }

    public void RemoveCar (Car car) {
      _spawnedCars.Remove(car);
    }
  }
}