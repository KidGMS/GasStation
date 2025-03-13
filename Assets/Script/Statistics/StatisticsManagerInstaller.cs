using Script.DI;
using Script.Statistics;
using UnityEngine;

namespace Script.Stat {
  public class StatisticsManagerInstaller : MonoBehaviour, IInstaller {
    private GlobalStatisticsManager _globalStatisticsManager;

    private void Awake() {
      _globalStatisticsManager = new GlobalStatisticsManager();
    }

    public void Install (DiContainer container) {
      container.Register<IStatisticsManager>(() => _globalStatisticsManager, Lifetime.Transient);
    }
  }
}