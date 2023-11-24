using DefaultNamespace;
using DG.Tweening;
using Event;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelFinishBlock : Block, IPoolable<IMemoryPool>
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventSystem.Raise(new FinishLineReachedEvent());
            }
        }

        protected override void OnInitialize()
        {
            transform.position = transform.position.ChangeY(-5f);
            transform.DOMoveY(0f, 0.25f);
        }

        public class Factory : PlaceholderFactory<LevelFinishBlock> { }
    }
}