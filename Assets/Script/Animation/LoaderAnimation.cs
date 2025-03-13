using System;
using System.Collections;
using System.Text;
using Script.LoadingSystem;
using TMPro;
using UnityEngine;

namespace Script.Animation {
  public class LoaderAnimation : MonoBehaviour, ITransitionAnimator {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private TextMeshProUGUI _loadingText;
    [SerializeField]
    private TextMeshProUGUI _loadingTextMain;

    private Coroutine _animationRoutine;
    private bool _isAnimating;
    private const int MAX_DOTS = 4;
    private readonly StringBuilder _textBuilder = new StringBuilder();

    private void Awake() {
      _canvasGroup.alpha = 0;
      _canvasGroup.blocksRaycasts = false;
    }

    public void PlayAnimation() {
      _canvasGroup.alpha = 1;
      _canvasGroup.blocksRaycasts = true;
      _isAnimating = true;

      if (_animationRoutine == null) {
        _animationRoutine = StartCoroutine(AnimateDots());
      }
    }

    public void StopAnimation() {
      _isAnimating = false;
      _textBuilder.Clear();
      _loadingText.text = String.Empty;
      _loadingTextMain.text = "Complete";

      if (_animationRoutine != null) {
        StopCoroutine(_animationRoutine);
        _animationRoutine = null;
      }
    }

    public void UpdateProgress (float progress) {}

    public void Finish() {
      StopAnimation();
    }

    private IEnumerator AnimateDots() {
      int dotCount = 0;

      while (_isAnimating) {
        dotCount = (dotCount + 1) % (MAX_DOTS + 1);
        _textBuilder.Clear();
        _textBuilder.Append('.', dotCount);

        _textBuilder.Append('\u00A0', MAX_DOTS - dotCount);

        _loadingText.text = _textBuilder.ToString();

        yield return new WaitForSeconds(0.1f);
      }
    }
  }
}