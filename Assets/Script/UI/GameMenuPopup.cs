using Script.DI;
using Script.Game;
using Script.LoadingSystem;
using Script.ObjectPools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI {
  public class GameMenuPopup : MonoBehaviour, IPoolable<GameMenuPopup> {
    [SerializeField]
    private Button _exit;
    [SerializeField]
    private Button _buttonClose;

    public IObjectPool<GameMenuPopup> Pool {
      get;
      set;
    }

    private void LoadMainScene() {
      ReturnToPool();
      SceneController sceneController = DiManager.Instance.Container.Resolve<SceneController>();
      sceneController.LoadScene(SceneType.Main);
    }

    public void OnPool() {
      _buttonClose.onClick.AddListener(ReturnToPool);
      _exit.onClick.AddListener(LoadMainScene);
    }

    public void ReturnToPool() {
      _buttonClose.onClick.RemoveListener(ReturnToPool);
      _exit.onClick.RemoveListener(LoadMainScene);
      gameObject.SetActive(false);
      Pool?.Release(this);
    }
  }
}