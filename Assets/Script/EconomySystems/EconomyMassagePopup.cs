using Script.ObjectPools;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Script.EconomySystems {
  public class EconomyMassagePopup : MonoBehaviour, IPoolable<EconomyMassagePopup> {
    [SerializeField]
    private TMP_Text _massageText;
    [SerializeField]
    private Button _buttonClose;

    public IObjectPool<EconomyMassagePopup> Pool {
      get;
      set;
    }

    public void SendMassage (string massage) {
      _massageText.text = massage;
    }

    public void OnPool() {
      _buttonClose.onClick.AddListener(ReturnToPool);
    }

    public void ReturnToPool() {
      _buttonClose.onClick.RemoveListener(ReturnToPool);
      gameObject.SetActive(false);
      Pool?.Release(this);
    }
  }
}