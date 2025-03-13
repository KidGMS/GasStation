using UnityEngine;

namespace Script.SaveSystem {
  public class SaveManager {
    private readonly ISaveSystem _saveSystem;

    public SaveManager() {
#if UNITY_STANDALONE || UNITY_EDITOR
      _saveSystem = new FileSaveSystem();
#elif UNITY_ANDROID || UNITY_IOS
            _saveSystem = new PrefsSaveSystem();
#else
            _saveSystem = new FileSaveSystem();
#endif
    }

    public void Save<T> (string key, T data) {
      _saveSystem.Save(key, data);
    }

    public T Load<T> (string key) {
      return _saveSystem.Load<T>(key);
    }

    public bool Exists (string key) {
      return _saveSystem.Exists(key);
    }

    public static void ClearData() {
      Debug.Log("SaveManager: Clearing ALL saved data...");
#if UNITY_STANDALONE || UNITY_EDITOR
      if (System.IO.Directory.Exists(Application.persistentDataPath)) {
        System.IO.Directory.Delete(Application.persistentDataPath, true);
        System.IO.Directory.CreateDirectory(Application.persistentDataPath);
        Debug.Log($"SaveManager: Cleared all files in {Application.persistentDataPath}");
      }
#elif UNITY_ANDROID || UNITY_IOS
      PlayerPrefs.DeleteAll();
      PlayerPrefs.Save();
      Debug.Log("SaveManager: Cleared PlayerPrefs.");
#endif
    }
  }
}