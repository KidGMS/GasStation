using Script.DI;
using Script.LoadingSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Game {
  public class GameSceneLoader : MonoBehaviour {
    [SerializeField]
    private Button _play;
    [SerializeField]
    private Button _buttonLeave;

    private void OnEnable() {
      _play.onClick.AddListener(OnStarts);
      _buttonLeave.onClick.AddListener(LeaveGame);
    }

    private void OnDisable() {
      _play.onClick.RemoveListener(OnStarts);
      _buttonLeave.onClick.RemoveListener(LeaveGame);
    }

    private void LeaveGame() {
      Application.Quit();
    }


    private void OnStarts() {
      SceneController sceneController = DiManager.Instance.Container.Resolve<SceneController>();
      sceneController.LoadScene(SceneType.Game);
      _play.onClick.RemoveListener(OnStarts);
    }
  }
}