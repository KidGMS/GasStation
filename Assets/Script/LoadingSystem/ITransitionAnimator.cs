namespace Script.LoadingSystem {
  public interface ITransitionAnimator {
    void PlayAnimation();

    void StopAnimation();

    void UpdateProgress (float progress);

    void Finish(); 
  }
}