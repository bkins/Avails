using System;

namespace Avails.D_Flat
{
    public static class FileSizeFormatter
    {
        public static string FormatSize(long bytes)  
        {  
            string[] suffixes    = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            var      suffixIndex = 0;  
            decimal  number      = bytes;  
        
            while (Math.Round(number / 1024) >= 1)  
            {  
                number /= 1024;  
                suffixIndex++;  
            }  
            return $"{number:n1}{suffixes[suffixIndex]}";  
        }  
    }
}