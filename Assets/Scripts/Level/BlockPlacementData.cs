using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "GamePlay Data", menuName = "GamePlay/GamePlay Data", order = 0)]
    public class BlockPlacementData : ScriptableObject
    {
        [SerializeField] private float _placementThreshold;
        [SerializeField] private int _blockSpawnDelay;
        [SerializeField] private int _blockCountPerLevel;

        public float PlacementThreshold => _placementThreshold;
        public int BlockSpawnDelay => _blockSpawnDelay;
        public int BlockCountPerLevel => _blockCountPerLevel;
    }
}