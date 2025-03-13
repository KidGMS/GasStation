using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.DI {
  public class DiManager : MonoBehaviour {
    public static DiManager Instance { get; private set; }
    public DiContainer Container { get; private set; }

    [SerializeField]
    private List<MonoBehaviour> _installers = new List<MonoBehaviour>();

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);

      Container = new DiContainer();

      foreach (MonoBehaviour installer in _installers) {
        if (installer is IInstaller dependencyInstaller) {
          dependencyInstaller.Install(Container);
        } else {
          Debug.LogWarning($"{installer.name} unrealizes IInstaller!");
        }
      }
    }
  }
}