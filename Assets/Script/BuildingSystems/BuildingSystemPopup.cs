using System;
using Script.DI;
using Script.EconomySystems;
using Script.ObjectPools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.BuildingSystems {
  public class BuildingSystemPopup : MonoBehaviour, IPoolable<BuildingSystemPopup> {
    [SerializeField]
    private TMP_Text _massageText;
    [SerializeField]
    private TMP_Text _costText;
    [SerializeField]
    private Button _buttonClose;
    [SerializeField]
    private Button _buildButton;

    public IObjectPool<BuildingSystemPopup> Pool {
      get;
      set;
    }

    public void SendMassage (string massage, string cost) {
      _massageText.text = massage;
      _costText.text = cost;
    }

    private void AddListener() {
      _buttonClose.onClick.AddListener(ReturnToPool);
      _buildButton.onClick.AddListener(Build);
    }

    private void Build() {
      DiManager.Instance.Container.Resolve<BuildManager>().ConstructBuilding();
    }

    private void RemoveListener() {
      _buttonClose.onClick.RemoveListener(ReturnToPool);
      _buildButton.onClick.RemoveListener(Build);
    }

    public void OnPool() {
      AddListener();
    }

    public void ReturnToPool() {
      RemoveListener();
      gameObject.SetActive(false);
      Pool?.Release(this);
    }
  }
}