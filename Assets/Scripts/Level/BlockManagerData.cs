using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "GamePlay Data", menuName = "GamePlay/GamePlay Data", order = 0)]
    public class BlockManagerData : ScriptableObject
    {
        [SerializeField] private float _placementThreshold;
        [SerializeField] private int _blockSpawnDelay;
        [SerializeField] private int _blockCountPerLevel;
        [SerializeField] private int _blockDespawnDelay;

        public float PlacementThreshold => _placementThreshold;
        public int BlockCountPerLevel => _blockCountPerLevel;
        public int BlockSpawnDelay => _blockSpawnDelay;
        public int BlockDespawnDelay => _blockDespawnDelay;
    }
}