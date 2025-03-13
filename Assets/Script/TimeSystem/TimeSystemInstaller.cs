using Script.DI;
using UnityEngine;

namespace Script.TimeSystem {
  public class TimeSystemInstaller : MonoBehaviour {
    [SerializeField]
    private TimeManager _timeManager;

    public void Awake() {
      DiManager.Instance.Container.Register(() => _timeManager, Lifetime.Singleton);
    }
  }
}