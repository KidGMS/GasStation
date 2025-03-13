using System;
using System.Collections.Generic;
using System.IO;
using Unity.Serialization.Json;
using UnityEngine;

namespace Script.SaveSystem {
  public class FileSaveSystem : ISaveSystem {
    private readonly string _saveFilePath;
    private Dictionary<string, string> _saveData;

    public FileSaveSystem() {
      _saveFilePath = Path.Combine(Application.persistentDataPath, SaveConstants.SAVE_FOLDER, "game_save.json");

      if (!Directory.Exists(Path.Combine(Application.persistentDataPath, SaveConstants.SAVE_FOLDER))) {
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, SaveConstants.SAVE_FOLDER));
      }

      LoadAllData();
    }

    public void Save<T> (string key, T data) {
      _saveData[key] = JsonSerialization.ToJson(data);
      SaveAllData();
    }

    public T Load<T> (string key) {
      if (_saveData.TryGetValue(key, out string jsonData)) {
        try {
          return JsonSerialization.FromJson<T>(jsonData);
        } catch (Exception ex) {
          Debug.LogError($"FileSaveSystem: Error when loading the key '{key}' - {ex.Message}");
        }
      }

      Debug.LogWarning($"FileSaveSystem: No data was found for the key '{key}'.");
      return default;
    }

    public bool Exists (string key) {
      return _saveData.ContainsKey(key);
    }

    private void LoadAllData() {
      if (File.Exists(_saveFilePath)) {
        try {
          string json = File.ReadAllText(_saveFilePath);
          _saveData = JsonSerialization.FromJson<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        } catch (Exception ex) {
          Debug.LogError($"FileSaveSystem: Error during data loading - {ex.Message}");
          _saveData = new Dictionary<string, string>();
        }
      } else {
        _saveData = new Dictionary<string, string>();
      }
    }

    private void SaveAllData() {
      try {
        string json = JsonSerialization.ToJson(_saveData);
        File.WriteAllText(_saveFilePath, json);
      } catch (Exception ex) {
        Debug.LogError($"FileSaveSystem: Error when saving data - {ex.Message}");
      }
    }
  }
}