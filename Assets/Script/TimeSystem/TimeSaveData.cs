using System;

namespace Script.TimeSystem {
  [Serializable]
  public class TimeSaveData {
    public float CurrentTime;
    public int DayCount;

    public TimeSaveData() {}

    public TimeSaveData (float time, int days) {
      CurrentTime = time;
      DayCount = days;
    }
  }
}