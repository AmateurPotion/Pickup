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

        public static Vector2 X(this Vector2 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector2 Y(this Vector2 vector2, float y)
        {
            vector2.y = y;
            return vector2;
        }
    }
}