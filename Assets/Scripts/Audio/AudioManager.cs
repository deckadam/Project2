using Event;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private AudioData _audioData;

        private int _perfectPlacementStreak;

        [Inject]
        private void Inject(AudioSource audioSource, AudioData audioData)
        {
            _audioSource = audioSource;
            _audioData = audioData;
        }

        private void OnEnable()
        {
            EventSystem.Subscribe<PerfectPlacementEvent>(OnPerfectPlacement);
            EventSystem.Subscribe<NonPerfectPlacementEvent>(NonPerfectPlacementEvent);
        }

        private void OnDisable()
        {
            EventSystem.Unsubscribe<PerfectPlacementEvent>(OnPerfectPlacement);
            EventSystem.Unsubscribe<NonPerfectPlacementEvent>(NonPerfectPlacementEvent);
        }

        private void NonPerfectPlacementEvent(object obj)
        {
            _perfectPlacementStreak = 0;
            PlayBuzzSound();
        }

        private void OnPerfectPlacement(object obj)
        {
            _perfectPlacementStreak++;
            PlayPerfectPlacementSound();
        }

        private void PlayPerfectPlacementSound()
        {
            _audioSource.clip = _audioData.PerfectPlacementSound;
            _audioSource.pitch = 1 + (_perfectPlacementStreak - 1) * _audioData.PitchIncementationAmount;
            _audioSource.Play();
        }

        private void PlayBuzzSound()
        {
            _audioSource.clip = _audioData.NonPerfectPlacementSound;
            _audioSource.pitch = 1f;
            _audioSource.Play();
        }
    }
}