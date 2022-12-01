using UnityEngine;

namespace Pickup.Utils.Override
{
    public static class Vector2Override
    {
        public static float GetAngle(this Vector2 start, Vector2 end)
        {
            var v2 = end - start;
            return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
        }
    }
}