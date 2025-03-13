using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoadSettings", menuName = "SO/Utility/SceneLoadSettings")]
public class SceneLoadSettings : ScriptableObject {
  public float LoaderDelay = 1f;
  public float LoadDelay = 2f;
  public float InitDelay = 0.5f;
  public float FinishDelay = 60f;
}