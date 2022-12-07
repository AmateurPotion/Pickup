using System.Text;

namespace Pickup.Utils.Override
{
    public static class ByteArrayOverride
    {
        public static string ByteToString(this byte[] arrayData) => arrayData.ByteToString(Encoding.Unicode);
        public static string ByteToString(this byte[] arrayData, Encoding encoding) => encoding.GetString(arrayData);
    }
}