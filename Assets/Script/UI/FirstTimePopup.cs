using Script.DI;
using Script.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI {
  public class FirstTimePopup : MonoBehaviour {
    private const string FIRST_TIME_KEY = "FirstTimeLaunch";
    [SerializeField]
    private GameObject _popupPanel;
    [SerializeField]
    private Button _closeButton;

    private SaveManager _saveManager;

    private void Start() {
      _saveManager = DiManager.Instance.Container.Resolve<SaveManager>();

      if (IsFirstTimeLaunch()) {
        ShowPopup();
      }
    }

    private bool IsFirstTimeLaunch() {
      return !_saveManager.Exists(FIRST_TIME_KEY);
    }

    private void ShowPopup() {
      _popupPanel.SetActive(true);
      _closeButton.onClick.AddListener(ClosePopup);
    }

    private void ClosePopup() {
      _popupPanel.SetActive(false);
      _closeButton.onClick.RemoveListener(ClosePopup);
      _saveManager.Save(FIRST_TIME_KEY, true);
    }
  }
}