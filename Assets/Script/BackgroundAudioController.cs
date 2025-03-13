using UnityEngine;

namespace Script {
  [RequireComponent(typeof(AudioSource))]
  public class BackgroundAudioController : MonoBehaviour {
    [SerializeField]
    private AudioClip _backgroundClip;
    [SerializeField, Range(0f, 1f)]
    private float _volume = 0.5f;

    private AudioSource _audioSource;

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();

      _audioSource.clip = _backgroundClip;
      _audioSource.loop = true;
      _audioSource.volume = _volume;
    }

    private void Start() {
      _audioSource.Play();
    }

    private void OnDestroy() {
      if (_audioSource.isPlaying) {
        _audioSource.Stop();
      }
    }
  }
}