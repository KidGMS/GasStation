using System.Collections.Generic;
using Script.DI;
using Script.ObjectPools;
using Script.SaveSystem;
using Script.Statistics;
using Script.UI;
using UnityEngine;

namespace Script.TimeSystem {
  public class TimeManager : MonoBehaviour {
    [SerializeField]
    private TimeModel _timeSettings;
    private readonly List<ITimeEvent> _timeEvents = new List<ITimeEvent>();

    private float _currentTime;
    private int _dayCount;
    private bool _isPaused;
    private float _timeMultiplier;

    private void Update() {
      if (_isPaused) {
        return;
      }

      _currentTime += Time.deltaTime * _timeMultiplier;

      if (_currentTime >= _timeSettings.EndHour * 60) {
        GameObject uiParent = GameObject.FindGameObjectWithTag("MainCanvas");

        if (uiParent != null) {
          PoolManager poolManager = DiManager.Instance.Container.Resolve<PoolManager>();
          DayStatisticsPopup popup = poolManager.GetObject<DayStatisticsPopup>(uiParent.transform);
          popup.Setup();
        }

        PauseTime(true);
      }

      CheckEvents();
    }

    private void OnEnable() {
      LoadTime();
      _timeMultiplier = _timeSettings.DefaultSpeed;
    }

    private void CheckEvents() {
      foreach (ITimeEvent timeEvent in _timeEvents) {
        timeEvent.OnTimeReached(_currentTime);
      }
    }

    public void RegisterEvent (ITimeEvent timeEvent) {
      _timeEvents.Add(timeEvent);
    }

    public void SetTimeMultiplier (float multiplier) {
      _timeMultiplier = multiplier;
    }

    public void PauseTime (bool isPaused) {
      _isPaused = isPaused;
    }

    public void StartNewDay() {
      _currentTime = _timeSettings.StartHour * 60;
      _dayCount++;
      SaveTime();
      PauseTime(false);
    }

    private void LoadTime() {
      SaveManager saveManager = DiManager.Instance.Container.Resolve<SaveManager>();

      if (saveManager.Exists(TimeConst.GAME_TIME_SAVE)) {
        TimeSaveData saveData = saveManager.Load<TimeSaveData>(TimeConst.GAME_TIME_SAVE);
        _currentTime = saveData.CurrentTime;
        _dayCount = saveData.DayCount;
      } else {
        _currentTime = _timeSettings.StartHour * 60;
        _dayCount = 1;
        SaveTime();
      }
    }

    private void SaveTime() {
      SaveManager saveManager = DiManager.Instance.Container.Resolve<SaveManager>();
      saveManager.Save(TimeConst.GAME_TIME_SAVE, new TimeSaveData(_currentTime, _dayCount));
    }

    public float GetCurrentTime() {
      return _currentTime;
    }

    public int GetDayCount() {
      return _dayCount;
    }

    public float GetEndTimeInMinutes() {
      return _timeSettings.EndHour * 60;
    }
  }
}