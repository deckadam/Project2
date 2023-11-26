using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Player/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float _playerForwardSpeed;
        [SerializeField] private float _playerHorizontalSpeed;
        [SerializeField] private float _cameraRotationDurationPerTurn;
        [SerializeField] private float _cameraFocusDuration;
        [SerializeField] private float _cameraVerticalAxisValue;

        public float PlayerForwardSpeed => _playerForwardSpeed;
        public float PlayerHorizontalSpeed => _playerHorizontalSpeed;
        public float CameraRotationDurationPerTurn => _cameraRotationDurationPerTurn;
        public float CameraFocusDuration => _cameraFocusDuration;
        public float CameraVerticalAxisValue => _cameraVerticalAxisValue;
    }
}