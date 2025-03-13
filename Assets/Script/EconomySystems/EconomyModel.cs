using Script.SaveSystem;

namespace Script.EconomySystems {
  public class EconomyModel : IEconomy{
    private readonly SaveManager _saveManager;
    private readonly EconomySettings _settings;
    private int _currentBalance;

    public int Balance {
      get {
        return _currentBalance;
      }
    }

    public EconomyModel (SaveManager saveManager, EconomySettings settings) {
      _saveManager = saveManager;
      _settings = settings;

      LoadBalance();
    }

    private void LoadBalance() {
      if (_saveManager.Exists(EconomyConstants.BALANCE_KEY)) {
        _currentBalance = _saveManager.Load<EconomyData>(EconomyConstants.BALANCE_KEY).Balance;
      } else {
        _currentBalance = _settings.DefaultBalance;
        SaveBalance();
      }
    }

    public void SaveBalance() {
      EconomyData data = new EconomyData(_currentBalance);
      _saveManager.Save(EconomyConstants.BALANCE_KEY, data);
    }


    public bool SpendMoney (int amount) {
      if (_currentBalance < amount) {
        return false;
      }
      
      _currentBalance -= amount;
      return true;
    }

    public void AddMoney (int amount) {
      _currentBalance += amount;
    }
  }
}