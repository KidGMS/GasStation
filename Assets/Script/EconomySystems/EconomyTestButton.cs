using Script.DI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.EconomySystems {
  public class EconomyTestButton : MonoBehaviour {
    [SerializeField]
    private Button _testButton;

    private EconomyManager _economyManager;

    private void Start() {
      _economyManager = DiManager.Instance.Container.Resolve<EconomyManager>();

      _testButton.onClick.AddListener(() => { _economyManager.GetPresenter().AddMoney(100); });
    }
  }
}