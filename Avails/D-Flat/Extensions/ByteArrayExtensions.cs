using System.Linq;

namespace Avails.D_Flat.Extensions
{
    public static class ByteArrayExtensions
    {
        public static bool SafeSequenceEqual(this byte[] first
                                           , byte[]      second)
        {
            if (first is null 
             && second is null)
            {
                return true;
            }

            if (first is null)
            {
                return false;
            }

            return first.SequenceEqual(second);
        }
    }
}