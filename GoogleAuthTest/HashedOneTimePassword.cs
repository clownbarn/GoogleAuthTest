using System;
using System.Security.Cryptography;
using System.Text;

namespace GoogleAuthTest
{
    /// <summary>
    /// Implementation for generating an HMAC-Based One-Time Password 
    /// based on RFC 4226.
    /// http://tools.ietf.org/html/rfc4226
    /// </summary>
    public class HashedOneTimePassword
    {
        #region Public Methods

        /// <summary>
        /// Generates an an HMAC-Based One-Time Password.
        /// </summary>
        /// <param name="secret">The shared secret.</param>
        /// <param name="iterationNumber">A counter if used in a counter based implementation, or 
        ///     derived from the current UTC time if using a time based implementation.</param>
        /// <param name="digits">The number of digits to return the the password.  Default is 6.</param>
        /// <returns></returns>
        public string GeneratePassword(string secret, long iterationNumber, int digits = 6)
        {            
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counter);

            byte[] key = Encoding.ASCII.GetBytes(secret);

            HMACSHA1 hmac = new HMACSHA1(key, true);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            // Convert the 4 bytes into an integer, ignoring the sign.
            int binary =
                ((hash[offset] & 0x7f) << 24)
                | (hash[offset + 1] << 16)
                | (hash[offset + 2] << 8)
                | (hash[offset + 3]);

            // Limit the number of digits
            int password = binary % (int)Math.Pow(10, digits);

            // Pad to required digits
            return password.ToString(new string('0', digits));            
        }

        #endregion
    }
}
