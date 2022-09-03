namespace Avails.D_Flat.Extensions
{
    public static class IntExtensions
    {
        public static string ToStringAsTwoDigits(this int value)
        {
            if (value > 99)
            {
                return value.ToString();
            }

            return value.ToString()
                        .PadLeft(2, '0');
        }
    }
}
