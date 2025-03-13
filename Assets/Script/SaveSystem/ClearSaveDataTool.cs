#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.SaveSystem {
  public static class ClearSaveDataTool {
    [MenuItem("Tools/Clear Save Data")]
    public static void ClearSaveData() {
      Debug.Log("ClearSaveDataTool: Clearing all saved data...");

      SaveManager.ClearData();

      if (Application.isPlaying) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
    }
  }
}

#endif