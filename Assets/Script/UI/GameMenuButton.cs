using Script.DI;
using Script.EconomySystems;
using Script.ObjectPools;
using UnityEngine;

namespace Script.UI {
  public class GameMenuButton : ControlPanelButtonBase {
    [SerializeField]
    private Transform _gameCanvas;

    protected override void ActionButton() {
      DiManager.Instance.Container.Resolve<PoolManager>().GetObject<GameMenuPopup>(_gameCanvas);
    }
  }
}