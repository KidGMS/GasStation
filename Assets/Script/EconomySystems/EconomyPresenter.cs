using UnityEngine;

namespace Script.EconomySystems {
  public class EconomyPresenter {
    private EconomyModel _model;
    private EconomyView _view;

    public void Initialize (EconomyModel model, EconomyView view) {
      _model = model;
      _view = view;
      UpdateUI();
    }

    public bool SpendMoney (int amount) {
      bool success = _model.SpendMoney(amount);

      if (success) {
        UpdateUI();
      } else {
        _view.ShowEconomyPopup(EconomyConstants.MASSAGE_NOT_ENOUGH_MONEY);
      }

      return success;
    }

    public void AddMoney (int amount) {
      _model.AddMoney(amount);
      UpdateUI();
    }

    private void UpdateUI() {
      _view.UpdateBalance(_model.Balance);
    }

    public void SaveBalance() {
      _model.SaveBalance();
    }
  }
}