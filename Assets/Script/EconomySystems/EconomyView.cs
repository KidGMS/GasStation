using TMPro;
using UnityEngine;
using System.Collections;
using Script.DI;
using Script.ObjectPools;

namespace Script.EconomySystems {
  public class EconomyView : MonoBehaviour {
    [SerializeField]
    private TMP_Text _balanceText;
    [SerializeField]
    private Transform _canvasContainer;

    public void UpdateBalance (int balance) {
      _balanceText.text = $"{balance}$";
    }

    public void ShowEconomyPopup (string massage) {
      EconomyMassagePopup economyMassagePopup = DiManager.Instance.Container.Resolve<PoolManager>().GetObject<EconomyMassagePopup>(_canvasContainer);
      economyMassagePopup.SendMassage(massage);
    }

  }
}