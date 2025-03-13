using System;
using UnityEngine;

namespace Script.LoadingSystem {
  [Serializable]
  public struct SceneItem {
    public string SceneName;
    public SceneType SceneType;
  }
}