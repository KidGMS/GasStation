using UnityEngine;

namespace Script.SaveSystem {
  public class PrefsSaveSystem : ISaveSystem {
    public void Save<T> (string key, T data) {
      string json = JsonUtility.ToJson(data);
      PlayerPrefs.SetString(key, json);
      PlayerPrefs.Save();
    }

    public T Load<T> (string key) {
      if (!PlayerPrefs.HasKey(key)) {
        return default;
      }

      string json = PlayerPrefs.GetString(key);
      return JsonUtility.FromJson<T>(json);
    }

    public bool Exists (string key) {
      return PlayerPrefs.HasKey(key);
    }
  }
}