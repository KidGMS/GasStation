using Script.DI;
using Script.SaveSystem;
using UnityEngine;

namespace Script.Installers {
  public class SaveManagerInstaller : MonoBehaviour, IInstaller {
    private SaveManager _saveManager;

    private void Awake() {
      _saveManager = new SaveManager();
    }

    public void Install (DiContainer container) {
      container.Register(() => _saveManager, Lifetime.Transient);
    }
  }
}