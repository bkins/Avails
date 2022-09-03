using System;
using System.ComponentModel;

namespace Avails.D_Flat.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumValue) 
        where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo   = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attributes.Length > 0)
                {
                    description = ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return description;
        }
    }
}