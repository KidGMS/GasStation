using Script.Animation;
using Script.DI;
using Script.LoadingSystem;
using UnityEngine;

namespace Script.Installers {
  public class LoaderAnimationInstaller : MonoBehaviour {
    [SerializeField]
    private LoaderAnimation _transitionAnimator;

    private void Awake() {
      DiManager.Instance.Container.Register<ITransitionAnimator>(() => _transitionAnimator, Lifetime.Transient);
    }
  }
}