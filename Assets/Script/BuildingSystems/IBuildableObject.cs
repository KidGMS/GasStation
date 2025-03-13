using Script.BuildingSystems;

namespace Script.Construction {
  public interface IBuildableObject {
    void PlaceAt (BuildZone zone);

    void Remove();

    ConstructionEntry GetSaveData(); 
  }
}