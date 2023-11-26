using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "Audio Data", menuName = "Audio/Audio Data", order = 0)]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private AudioClip _perfectPlacementSound;
        [SerializeField] private AudioClip _nonPerfectPlacementSound;
        [SerializeField] private float _pitchIncementationAmount;

        public AudioClip PerfectPlacementSound => _perfectPlacementSound;
        public AudioClip NonPerfectPlacementSound => _nonPerfectPlacementSound;
        public float PitchIncementationAmount => _pitchIncementationAmount;
    }
}