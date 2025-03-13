using System;
using UnityEngine;

namespace Script.UI {
  public class ControlPanel : MonoBehaviour {
    [SerializeField]
    private ControlPanelButtonBase [] _controlPanelButtons;

    private void OnEnable() {
      foreach (ControlPanelButtonBase controlPanelButton in _controlPanelButtons) {
        controlPanelButton.AddListener();
      }
    }

    private void OnDisable() {
      foreach (ControlPanelButtonBase controlPanelButton in _controlPanelButtons) {
        controlPanelButton.RemoveListener();
      }
    }
  }
}