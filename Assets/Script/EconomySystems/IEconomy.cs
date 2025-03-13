namespace Script.EconomySystems {
  public interface IEconomy {
    int Balance { get; }

    bool SpendMoney (int amount);

    void AddMoney (int amount);
  }
}