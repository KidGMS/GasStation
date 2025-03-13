using Script.DI;
using Script.ObjectPools;
using UnityEngine;

namespace Script.Installers {
  public class PoolManagerInstaller : MonoBehaviour, IInstaller {
    [SerializeField]
    private PoolManager _poolManager;

    public void Install (DiContainer container) {
      if (_poolManager != null) {
        container.Register(() => _poolManager, Lifetime.Singleton);
      }
    }
  }
}