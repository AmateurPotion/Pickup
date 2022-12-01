using System.Text;

namespace Pickup.Utils.Override
{
    public static class StringOverride
    {
        public static byte[] ToByteArray(this string text) => text.ToByteArray(Encoding.Unicode);
        public static byte[] ToByteArray(this string text, Encoding encoding) => encoding.GetBytes(text);
    }
}