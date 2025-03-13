using UnityEngine;

namespace Script.TimeSystem {
  public class TimePresenter {
    private readonly TimeManager _timeManager;
    private readonly TimeView _timeView;
    private float _lastUpdateTime;

    public TimePresenter (TimeManager timeManager, TimeView timeView) {
      _timeManager = timeManager;
      _timeView = timeView;
      _lastUpdateTime = 0f;
    }

    public void UpdateUI() {
      float time = _timeManager.GetCurrentTime();

      if (!(Time.time - _lastUpdateTime >= 1f)) {
        return;
      }

      int hours = (int)(time / 60);
      int minutes = (int)(time % 60);
      _timeView.UpdateTimeUI(hours, minutes);
      _timeView.UpdateDayUI(_timeManager.GetDayCount());

      _lastUpdateTime = Time.time;
    }
  }
}