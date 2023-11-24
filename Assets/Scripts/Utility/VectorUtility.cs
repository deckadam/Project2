using UnityEngine;

namespace DefaultNamespace
{
    public static class VectorUtility
    {
        public static Vector3 ChangeX(this Vector3 data, float x)
        {
            return new Vector3(x, data.y, data.z);
        }
        
        public static Vector3 ChangeY(this Vector3 data, float y)
        {
            return new Vector3(data.x, y, data.z);
        }
    }
}