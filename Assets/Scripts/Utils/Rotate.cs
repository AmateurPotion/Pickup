using System.Numerics;

namespace Pickup.Utils
{
    public static class Rotate2D
    {
        public static readonly Vector3 Up = new(0, 0, 0);
        public static readonly Vector3 Down = new(0, 0, -180);
        public static readonly Vector3 Left = new(0, 0, 90);
        public static readonly Vector3 Right = new(0, 0, -90);
        
        public static readonly Vector3 UpLeft = new(0, 0, 45);
        public static readonly Vector3 DownLeft = new(0, 0, -225);
        public static readonly Vector3 UpRight = new(0, 0, -45);
        public static readonly Vector3 DownRight = new(0, 0, -135);
    }
}