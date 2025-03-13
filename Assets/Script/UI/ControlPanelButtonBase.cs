using UnityEngine;
using UnityEngine.UI;

namespace Script.UI {
  public class ControlPanelButtonBase : MonoBehaviour {
    [SerializeField]
    private Button _button;

    public void AddListener() {
      _button.onClick.AddListener(ActionButton);
    }

    public void RemoveListener() {
      _button.onClick.AddListener(ActionButton);
    }

    protected virtual void ActionButton() {}
  }
}