using Script.DI;
using UnityEngine;

namespace Script.TimeSystem {
  public class TimeController : MonoBehaviour {
    private TimePresenter _presenter;
    private TimeManager _timeManager;
    private TimeView _timeView;

    private void Start() {
      _timeManager = DiManager.Instance.Container.Resolve<TimeManager>();
      _timeView = FindObjectOfType<TimeView>();

      _presenter = new TimePresenter(_timeManager, _timeView);
    }

    private void Update() {
      _presenter.UpdateUI(); 
    }
  }
}