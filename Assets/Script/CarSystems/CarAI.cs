using System.Collections.Generic;
using Script.DI;
using Script.FuelSystem;
using Script.Stat;
using Script.Statistics;
using Script.WaypointSystems;
using UnityEngine;

namespace Script.CarSystems {
  public class CarAI : MonoBehaviour {
    private const float FUEL_THRESHOLD = 0.8f;
    private Car _car;
    private CarAIData _carAIData;
    private FuelPump _currentPump;
    private float _currentSpeed;
    private Waypoint _currentWaypoint;
    private bool _isQueued;
    private bool _isRefueling;
    private Vector3 _queueTarget;

    private bool IsWaypointNull {
      get { return _currentWaypoint == null; }
    }
    private bool IsDespawnWaypoint {
      get { return _currentWaypoint?.Type == WaypointType.Despawn; }
    }
    private bool HasNextWaypoints {
      get { return _currentWaypoint?.NextWaypoints.Count > 0; }
    }
    private bool IsEntryOrGasAltEntry {
      get { return _currentWaypoint?.Type == WaypointType.Entry || _currentWaypoint?.Type == WaypointType.GasAltEntry; }
    }
    private bool IsGasEntry {
      get { return _currentWaypoint?.Type == WaypointType.GasEntry; }
    }

    private void Update() {
      if (IsWaypointNull) {
        return;
      }

      if (_isRefueling) {
        return;
      }

      if (_isQueued) {
        MoveTowardsQueueTarget();
        return;
      }

      HandleCollisions();
      MoveToWaypoint();
    }

    private void OnDrawGizmosSelected() {
      if (_carAIData == null) {
        return;
      }

      Gizmos.color = Color.red;
      Vector3 boxSize = new Vector3(2f, 1f, _carAIData.DetectionRange);
      Vector3 boxCenter = transform.position + transform.forward * (_carAIData.DetectionRange / 2);
      Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    public void Initialize (Car car, CarAIData aiData) {
      _car = car;
      _carAIData = aiData;
      _currentSpeed = car.CarData.Speed;
    }

    public void SetStartWaypoint (Waypoint startWaypoint) {
      _currentWaypoint = startWaypoint;
      transform.position = startWaypoint.transform.position;
    }

    private void MoveToWaypoint() {
      Vector3 direction = (_currentWaypoint.transform.position - transform.position).normalized;
      transform.position += direction * _currentSpeed * Time.deltaTime;

      if (direction != Vector3.zero) {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _carAIData.TurnSpeed);
      }

      if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < _carAIData.StopDistance) {
        HandleWaypoint();
      }
    }

    private void HandleWaypoint() {
      if (IsWaypointNull || !HasNextWaypoints) {
        if (!IsDespawnWaypoint) {
          return;
        }

        DespawnCar();
        return;
      }

      if (IsEntryOrGasAltEntry) {
        DecideRefuelOrContinue();
      } else if (IsGasEntry) {
        DecideGasPumpOrContinue();
      } else {
        MoveToNextWaypoint();
      }
    }

    private void DecideRefuelOrContinue() {
      float fuelLevel = _car.CarData.CurrentFuel / _car.CarData.MaxFuel;

      if (fuelLevel < FUEL_THRESHOLD) {
        SetNextWaypoint(WaypointType.GasRoad);
      } else {
        SetNextWaypoint(WaypointType.Road);
      }
    }

    private void DecideGasPumpOrContinue() {
      if (_currentWaypoint is WaypointGasEntry) {
        FuelPump pump = (_currentWaypoint as WaypointGasEntry).FuelPump;

        if (pump != null && pump.CanRefuel(_car) && CarAcceptsPrice(pump)) {
          _currentPump = pump;

          if (!pump.IsCarEnqueued(_car)) {
            pump.EnqueueCar(_car);
          }

          int queueIndex = pump.GetQueueIndex(_car);
          float spacing = 3f;
          Waypoint gasRefuelingWaypoint = GetNextWaypoint(WaypointType.GasRefueling);

          if (gasRefuelingWaypoint == null) {
            SetNextWaypoint(WaypointType.GasRoad);
            return;
          }

          _currentWaypoint = gasRefuelingWaypoint;
          float distanceToRefuel = Vector3.Distance(transform.position, gasRefuelingWaypoint.transform.position);

          if (distanceToRefuel > _carAIData.StopDistance * 2) {
            _queueTarget = gasRefuelingWaypoint.transform.position - gasRefuelingWaypoint.transform.forward * spacing * queueIndex;
            _isQueued = true;
            _isRefueling = false;
          } else {
            _queueTarget = gasRefuelingWaypoint.transform.position;
            _isRefueling = true;
            _isQueued = false;
          }

          return;
        } else {
          var statsManager = DiManager.Instance.Container.Resolve<IStatisticsManager>();
          statsManager.AddFuelingData(0, 0, false, true);
        }
      }

      SetNextWaypoint(WaypointType.GasRoad);
    }

    public void OnRefueled() {
      _isRefueling = false;
      _isQueued = false;
      SetNextWaypoint(WaypointType.GasRoad);
    }

    private bool CarAcceptsPrice (FuelPump pump) {
      if (pump.FuelPrice > pump.BaseFuelPrice * 4f) {
        return false;
      }

      float priceDifference = pump.FuelPrice - pump.BaseFuelPrice;
      float pricePercentage = priceDifference / pump.BaseFuelPrice * 100f;
      float rejectionChance = pricePercentage / 100f * (1f - _car.CarData.Satisfaction / 100f);
      return Random.value > rejectionChance;
    }


    private Waypoint GetNextWaypoint (WaypointType type) {
      foreach (Waypoint next in _currentWaypoint.NextWaypoints) {
        if (next.Type == type) {
          return next;
        }
      }

      return null;
    }

    private void SetNextWaypoint (WaypointType preferredType) {
      List<Waypoint> possibleWaypoints = new List<Waypoint>();

      foreach (Waypoint next in _currentWaypoint.NextWaypoints) {
        if (next.Type == preferredType) {
          possibleWaypoints.Add(next);
        }
      }

      _currentWaypoint = possibleWaypoints.Count > 0 ? possibleWaypoints[Random.Range(0, possibleWaypoints.Count)]
        : _currentWaypoint.NextWaypoints[Random.Range(0, _currentWaypoint.NextWaypoints.Count)];
    }

    private void MoveToNextWaypoint() {
      if (!HasNextWaypoints) {
        return;
      }

      _currentWaypoint = _currentWaypoint.NextWaypoints[Random.Range(0, _currentWaypoint.NextWaypoints.Count)];
    }

    private void MoveTowardsQueueTarget() {
      if (_currentPump != null && _currentWaypoint != null) {
        int queueIndex = _currentPump.GetQueueIndex(_car);
        float spacing = 15f;
        _queueTarget = _currentWaypoint.transform.position - _currentWaypoint.transform.forward * spacing * queueIndex;
      }

      float distanceToTarget = Vector3.Distance(transform.position, _queueTarget);

      if (distanceToTarget < 0.1f) {
        return;
      }

      Vector3 direction = (_queueTarget - transform.position).normalized;
      transform.position += direction * _currentSpeed * Time.deltaTime;

      if (direction != Vector3.zero) {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _carAIData.TurnSpeed);
      }
    }

    private void DespawnCar() {
      _car.ReturnToPool();
    }

    private void HandleCollisions() {
      Vector3 boxSize = new Vector3(_carAIData.CollisionBoxWidth, _carAIData.CollisionBoxHeight, _carAIData.DetectionRange);
      Vector3 boxCenter = transform.position + transform.forward * (_carAIData.DetectionRange / 2);
      Collider [] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, transform.rotation);
      bool isCarTooClose = false;
      float stopDistance = _carAIData.StopDistance * _carAIData.StopDistanceMultiplier;
      float targetSpeed = _car.CarData.Speed;

      foreach (Collider hit in hitColliders) {
        if (!hit.CompareTag("Car") || hit.gameObject == gameObject) {
          continue;
        }

        Vector3 toOtherCar = (hit.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, hit.transform.position);
        float forwardDot = Vector3.Dot(transform.forward, toOtherCar);

        if (!(forwardDot > 0.5f) || !(distance < stopDistance)) {
          continue;
        }

        targetSpeed = 0f;
        isCarTooClose = true;
        break;
      }

      AdjustSpeed(targetSpeed, isCarTooClose);
    }

    private void AdjustSpeed (float targetSpeed, bool isBraking) {
      float lerpFactor = isBraking ? _carAIData.BrakingLerpSpeed : _carAIData.AccelerationLerpSpeed;
      _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * lerpFactor);
    }
  }
}