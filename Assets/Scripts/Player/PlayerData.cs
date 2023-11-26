using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Player/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _playerForwardSpeed;
        [SerializeField] private float _playerHorizontalSpeed;

        [Header("Camera")]
        [SerializeField] private float _cameraRotationDurationPerTurn;
        [SerializeField] private float _cameraFocusDuration;
        [SerializeField] private float _cameraVerticalAxisValue;

        [Header("Fall Animation")]
        [SerializeField] private Vector3 _fallPosition;
        [SerializeField] private float _fallDuration;
        [SerializeField] private float _fallPower;


        public float PlayerForwardSpeed => _playerForwardSpeed;
        public float PlayerHorizontalSpeed => _playerHorizontalSpeed;

        public float CameraRotationDurationPerTurn => _cameraRotationDurationPerTurn;
        public float CameraFocusDuration => _cameraFocusDuration;
        public float CameraVerticalAxisValue => _cameraVerticalAxisValue;

        public Vector3 FallPosition => _fallPosition;
        public float FallDuration => _fallDuration;
        public float FallPower => _fallPower;
    }
}