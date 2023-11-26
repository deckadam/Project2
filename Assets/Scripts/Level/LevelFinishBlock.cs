using Event;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelFinishBlock : Block
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventSystem.Raise(new FinishLineReachedEvent());
            }
        }

        public class Factory : PlaceholderFactory<LevelFinishBlock> { }
    }
}