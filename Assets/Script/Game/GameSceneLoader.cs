using Script.DI;
using Script.LoadingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Game {
  public class GameSceneLoader : MonoBehaviour {
    [SerializeField]
    private Button _play;

    private void OnEnable() {
      _play.onClick.AddListener(OnStarts);
    }

    private void OnDisable() {
      _play.onClick.RemoveListener(OnStarts);
    }


    private void OnStarts() {
      SceneController sceneController = DiManager.Instance.Container.Resolve<SceneController>();
      sceneController.LoadScene(SceneType.Game);
      _play.onClick.RemoveListener(OnStarts);
    }
  }
}