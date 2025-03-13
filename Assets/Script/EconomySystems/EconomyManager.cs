using Script.DI;
using Script.SaveSystem;
using UnityEngine;

namespace Script.EconomySystems {
  public class EconomyManager : MonoBehaviour {
    [SerializeField]
    private EconomySettings _economySettings;
    [SerializeField]
    private EconomyView _economyView;

    private EconomyPresenter _presenter;
    private EconomyModel _model;
    private bool _isQuitting = false;

    private void OnEnable() {
      _model = new EconomyModel(DiManager.Instance.Container.Resolve<SaveManager>(),_economySettings);
      _presenter = new EconomyPresenter();
      _presenter.Initialize(_model, _economyView);
    }

    private void OnDisable() {
      if (!_isQuitting) {
        _presenter.SaveBalance();
      }
    }

    private void OnApplicationQuit() {
      _isQuitting = true;
      _presenter.SaveBalance();
    }

    public EconomyPresenter GetPresenter() {
      return _presenter;
    }
  }
}