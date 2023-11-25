using System;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Player/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private PlayerMovementData _playerMovementData;

        public PlayerMovementData MovementData => _playerMovementData;

        [Serializable]
        public class PlayerMovementData
        {
            [SerializeField] private float _playerForwardSpeed;
            [SerializeField] private float _playerHorizontalSpeed;
            
            public float PlayerForwardSpeed => _playerForwardSpeed;
            public float PlayerHorizontalSpeed => _playerHorizontalSpeed;
        }
    }
}