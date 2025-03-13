using Script.DI;
using Script.ObjectPools;
using Script.TimeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Statistics {
  public class DayStatisticsPopup : MonoBehaviour, IPoolable<DayStatisticsPopup> {
    [SerializeField]
    private TMP_Text _dayText;
    [SerializeField]
    private TMP_Text _revenueText;
    [SerializeField]
    private TMP_Text _fuelText;
    [SerializeField]
    private TMP_Text _servedText;
    [SerializeField]
    private TMP_Text _leftText;
    [SerializeField]
    private Button _newDayButton;

    public IObjectPool<DayStatisticsPopup> Pool { get; set; }

    public void OnPool() {
      _newDayButton.onClick.AddListener(OnStartNewDay);
    }

    public void ReturnToPool() {
      _newDayButton.onClick.RemoveListener(OnStartNewDay);
      gameObject.SetActive(false);
      Pool?.Release(this);
    }

    public void Setup() {
      IStatisticsManager statsManager = DiManager.Instance.Container.Resolve<IStatisticsManager>();
      TimeManager timeManager = DiManager.Instance.Container.Resolve<TimeManager>();
      DayStatistics stats = statsManager.GetCurrentDayStatistics();
      int currentDay = timeManager.GetDayCount();

      _dayText.text = $"Day {currentDay} Summary";
      _revenueText.text = $"Total Revenue: {stats.TotalRevenue}";
      _fuelText.text = $"Total Gallons: {stats.TotalGallons}";
      _servedText.text = $"Clients Served: {stats.ClientsServed}";
      _leftText.text = $"Clients Left: {stats.ClientsLeft}";
      gameObject.SetActive(true);
    }

    private void OnStartNewDay() {
      IStatisticsManager statsManager = DiManager.Instance.Container.Resolve<IStatisticsManager>();
      statsManager.ResetStatistics();
      TimeManager timeManager = DiManager.Instance.Container.Resolve<TimeManager>();
      timeManager.StartNewDay();
      ReturnToPool();
    }
  }
}