using System;
using System.Text;

namespace AccidentalFish.ApplicationSupport.Core.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Base 64 encode a string
        /// </summary>
        /// <param name="plainText">The string to encode</param>
        /// <returns>The encoded string</returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base 64 decode a string
        /// </summary>
        /// <param name="base64EncodedData">The encoded data</param>
        /// <returns>The decoded data</returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
