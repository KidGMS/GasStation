using Script.BuildingSystems;
using UnityEngine;
using Script.ObjectPools;

namespace Script.Construction {
  public class Building : MonoBehaviour, IBuildableObject, IPoolable<Building> {
    public IObjectPool<Building> Pool { get; set; }
    private BuildZone _currentZone;
    private int _level;
    private string _owner;

    public void Initialize (int level, string owner) {
      _level = level;
      _owner = owner;
    }

    public void PlaceAt (BuildZone zone) {
      _currentZone = zone;
      transform.position = zone.BuildPosition;
      zone.MarkAsBuilt();
    }

    public void Remove() {
      _currentZone.MarkAsEmpty();
      ReturnToPool();
    }

    public ConstructionEntry GetSaveData() {
      return new ConstructionEntry {
        Position = _currentZone.transform.position,
        Transform = _currentZone.transform,
        PrefabID = gameObject.GetInstanceID(),
        Level = _level,
        Owner = _owner
      };
    }


    public void OnPool() {}

    public void ReturnToPool() {
      gameObject.SetActive(false);
      Pool?.Release(this);
    }
  }
}