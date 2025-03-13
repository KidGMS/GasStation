using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Script.DI;

namespace Script.LoadingSystem {
  public class SceneController {
    private readonly ISceneProvider _sceneProvider;
    private readonly DiContainer _diContainer;
    private readonly MonoBehaviour _coroutineRunner;
    private ITransitionAnimator _transitionAnimator;
    private string _currentScene;
    private Type [] _requiredComponents;
    private string _loadingScene;
    private string _targetScene;
    private AsyncOperation _asyncLoad;
    private readonly SceneLoadSettings _settings;
    private Action _finishLoad;

    public SceneController (ISceneProvider sceneProvider, DiContainer diContainer, SceneLoadSettings settings, MonoBehaviour coroutineRunner) {
      _sceneProvider = sceneProvider;
      _diContainer = diContainer;
      _coroutineRunner = coroutineRunner;
      _settings = settings;
    }

    public void LoadScene (SceneType targetSceneType, Action finishLoad = null, params Type [] requiredComponents) {
      _currentScene = SceneManager.GetActiveScene().name;
      _loadingScene = _sceneProvider.GetSceneByType(SceneType.Loader);
      _targetScene = _sceneProvider.GetSceneByType(targetSceneType);
      _finishLoad = finishLoad;

      if (string.IsNullOrEmpty(_loadingScene) || string.IsNullOrEmpty(_targetScene)) {
        return;
      }

      _requiredComponents = requiredComponents;
      _coroutineRunner.StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine() {
      yield return LoadLoaderScene();
      yield return UnloadPreviousScene();
      yield return LoadTargetScene();
    }

    private IEnumerator LoadLoaderScene() {
      yield return SceneManager.LoadSceneAsync(_loadingScene, LoadSceneMode.Additive);

      if (RegisterAnimation()) {
        PlayAnimation();
      }

      yield return new WaitForSeconds(_settings.LoaderDelay);
    }

    private bool RegisterAnimation() {
      if (!_diContainer.IsRegistered<ITransitionAnimator>()) {
        return false;
      }

      _transitionAnimator = _diContainer.Resolve<ITransitionAnimator>();
      return true;

    }

    private void PlayAnimation() {
      _transitionAnimator?.PlayAnimation();
    }

    private IEnumerator UnloadPreviousScene() {
      if (!string.IsNullOrEmpty(_currentScene)) {
        yield return SceneManager.UnloadSceneAsync(_currentScene);
      }
    }

    private IEnumerator LoadTargetScene() {
      _asyncLoad = SceneManager.LoadSceneAsync(_targetScene, LoadSceneMode.Additive);
      _asyncLoad.allowSceneActivation = false;

      while (_asyncLoad.progress < 0.9f) {
        _transitionAnimator?.UpdateProgress(_asyncLoad.progress * 0.5f);
        yield return null;
      }

      yield return new WaitForSeconds(_settings.LoadDelay);

    

      yield return FinalizeLoading();
    }



    private IEnumerator InitializeComponents() {
      int totalComponents = _requiredComponents.Length;
      int initialized = 0;

      foreach (Type componentType in _requiredComponents) {
        ISceneLoadHandler component = _diContainer.Resolve(componentType) as ISceneLoadHandler;
        component?.OnSceneLoaded();
        initialized++;

        _transitionAnimator?.UpdateProgress(0.5f + initialized / (float)totalComponents * 0.5f);
      }

      yield return new WaitForSeconds(_settings.InitDelay);
    }

    private IEnumerator FinalizeLoading() {
      _transitionAnimator?.StopAnimation();
      _transitionAnimator?.Finish();
      yield return new WaitForSeconds(_settings.FinishDelay);

      _asyncLoad.allowSceneActivation = true;

      while (!_asyncLoad.isDone) {
        yield return null;
      }

      Scene newScene = SceneManager.GetSceneByName(_targetScene);

      if (newScene.IsValid()) {
        SceneManager.SetActiveScene(newScene);
      } else {
        yield break;
      }
      
      yield return InitializeComponents();
      yield return SceneManager.UnloadSceneAsync(_loadingScene);

      _finishLoad?.Invoke();
      _currentScene = _targetScene;
    }



  }
}