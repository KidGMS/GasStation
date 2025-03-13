using Script.BuildingSystems;
using Script.DI;
using Script.Construction;
using Script.UI;
using UnityEngine;

namespace Script.Installers {
  public class BuildSystemInstaller : MonoBehaviour {
    [SerializeField]
    private BuildManager _buildManager;
    [SerializeField]
    private BuildSettings _buildSettings;

    public void Awake() {
      DiManager.Instance.Container.Register(() => _buildManager, Lifetime.Singleton);
      DiManager.Instance.Container.Register(() => _buildSettings, Lifetime.Singleton);
    }
  }
}