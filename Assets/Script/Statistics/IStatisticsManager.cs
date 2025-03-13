namespace Script.Statistics {
  public interface IStatisticsManager {
    void AddFuelingData (float revenue, float gallons, bool served, bool clientLeft);

    DayStatistics GetCurrentDayStatistics();

    void ResetStatistics();
  }

  public class DayStatistics {
    public float TotalRevenue;
    public float TotalGallons;
    public int ClientsServed;
    public int ClientsLeft;

    public DayStatistics (float revenue, float gallons, int served, int left) {
      TotalRevenue = revenue;
      TotalGallons = gallons;
      ClientsServed = served;
      ClientsLeft = left;
    }
  }
}