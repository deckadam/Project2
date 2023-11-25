using Zenject;

namespace Level
{
    public class MovingBlock : Block, IPoolable<IMemoryPool>
    {
        public class Factory : PlaceholderFactory<MovingBlock> { }
    }
}