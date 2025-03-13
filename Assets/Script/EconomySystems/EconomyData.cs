namespace Script.EconomySystems {
  [System.Serializable]
  public class EconomyData {
    public int Balance;

    public EconomyData() {}

    public EconomyData (int balance) {
      Balance = balance;
    }
  }
}