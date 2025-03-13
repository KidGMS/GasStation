using TMPro;
using UnityEngine;

namespace Script.TimeSystem {
  public class TimeView : MonoBehaviour {
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _dayText;

    public void UpdateTimeUI(int hours, int minutes) {
      _timeText.text = $"{hours:D2}:{minutes:D2}";
    }

    public void UpdateDayUI(int day) {
      _dayText.text = $"Day: {day}";
    }
  }
}