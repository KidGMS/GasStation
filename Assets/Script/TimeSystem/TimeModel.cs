using UnityEngine;
using UnityEngine.Serialization;

namespace Script.TimeSystem
{
    [CreateAssetMenu(fileName = "TimeSettings", menuName = "SO/Settings/Time Settings")]
    public class TimeModel : ScriptableObject
    {
        public float DefaultSpeed = 1f;
        public int StartHour = 6;
        public int EndHour = 24;
    }
}