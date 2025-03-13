using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script.LoadingSystem {
  [CreateAssetMenu(fileName = "SceneData", menuName = "SO/Utility/SceneData")]
  public class SceneData : ScriptableObject, ISceneProvider {
    [SerializeField]
    private List<SceneItem> _sceneItems;

    private Dictionary<SceneType, string> _sceneDictionary;

    public void Initialize() {
      if (_sceneDictionary != null) {
        return;
      }

      _sceneDictionary = _sceneItems.ToDictionary(item => item.SceneType, item => item.SceneName);
    }

    public string GetSceneByType (SceneType type) {
      Initialize();
      return _sceneDictionary.TryGetValue(type, out string sceneName) ? sceneName : null;
    }
  }
}