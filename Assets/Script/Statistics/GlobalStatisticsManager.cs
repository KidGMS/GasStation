using Script.Stat;

namespace Script.Statistics {
  public class GlobalStatisticsManager : IStatisticsManager {
    private int _clientsLeft;
    private int _clientsServed;
    private float _totalGallons;
    private float _totalRevenue;

    public void AddFuelingData (float revenue, float gallons, bool served, bool clientLeft) {
      _totalRevenue += revenue;
      _totalGallons += gallons;

      if (served) {
        _clientsServed++;
      }

      if (clientLeft) {
        _clientsLeft++;
      }
    }

    public DayStatistics GetCurrentDayStatistics() {
      return new DayStatistics(_totalRevenue, _totalGallons, _clientsServed, _clientsLeft);
    }

    public void ResetStatistics() {
      _totalRevenue = 0;
      _totalGallons = 0;
      _clientsServed = 0;
      _clientsLeft = 0;
    }
  }
}