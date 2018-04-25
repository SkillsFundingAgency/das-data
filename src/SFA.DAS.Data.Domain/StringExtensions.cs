using System;

namespace SFA.DAS.Data.Domain
{
    public static class StringExtensions
    {
        public static Uri ToUri(this string theUrl)
        {
            return new Uri(theUrl);
        }
    }
}
