using Script.DI;
using Script.EconomySystems;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Installers {
  public class EconomyManagerInstaller : MonoBehaviour {
    [SerializeField]
    private EconomyManager _economyManager;

    private void Awake() {
      DiManager.Instance.Container.Register(() => _economyManager, Lifetime.Singleton);
    }
  }
}