using System;
using Script.DI;
using UnityEngine;

namespace Script.LoadingSystem {
  public class SceneLoaderInstaller : MonoBehaviour, IInstaller {
    [SerializeField]
    private SceneData _sceneData;
    [SerializeField]
    private SceneLoadSettings _sceneLoadSettings;

    public void Install (DiContainer container) {
      container.Register(_sceneData);
      container.Register(_sceneLoadSettings);

      SceneController sceneController = new SceneController(_sceneData, container, _sceneLoadSettings, this);
      container.Register(() => sceneController, Lifetime.Singleton);

      sceneController.LoadScene(SceneType.Main);
    }
  }
}