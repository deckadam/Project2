using UnityEngine;
using Zenject;

namespace Level
{
    public class Block : MonoBehaviour
    {
        [SerializeField] protected float _size;

        protected bool _isOnRightSize;
        protected bool _isLockedIn;
        private IMemoryPool _pool;

        public void Despawn()
        {
            _pool.Despawn(this);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            gameObject.SetActive(true);
        }


        public void Initialize(float z, bool isOnRightSide, bool isLockedIn)
        {
            _isOnRightSize = isOnRightSide;
            _isLockedIn = isLockedIn;
            
            var xValue = 0f;
            if (!isLockedIn)
            {
                xValue = isOnRightSide ? -10f : 10f;
            }

            var placementPosition = new Vector3(xValue, 0, z);
            transform.position = placementPosition;

            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public float GetSize() => _size;
    }
}