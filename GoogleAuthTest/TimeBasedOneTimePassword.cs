using System;
using System.Runtime.Caching;

namespace GoogleAuthTest
{
    /// <summary>
    /// Implementation for generating a Time-Based One-Time Password 
    /// based on RFC 6238.
    /// http://tools.ietf.org/html/rfc4226
    /// </summary>
    public class TimeBasedOneTimePassword
    {
        #region Private Fields

        private readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly HashedOneTimePassword _hotp = null; 
        
        #endregion
        
        #region Constructor

        public TimeBasedOneTimePassword()
        {
            _hotp = new HashedOneTimePassword();
        }        

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a Time-Based One-Time Password for a given shared secret.
        /// </summary>
        /// <param name="secret">The shared secret.</param>
        /// <returns></returns>
        public string GetPassword(string secret, int digits = 6)
        {
            return GetPassword(secret, GetCurrentCounter());
        }

        public string GetPassword(string secret, long counter, int digits = 6)
        {
            return _hotp.GeneratePassword(secret, counter, digits);
        }                
        
        public long GetCurrentCounter(int timeStep = 30)
        {
            return GetCurrentCounter(DateTime.UtcNow, UNIX_EPOCH, timeStep);
        }        

        #endregion        

        #region Private Methods
        
        private long GetCurrentCounter(DateTime now, DateTime epoch, int timeStep)
        {
            return (long)(now - epoch).TotalSeconds / timeStep;
        }      
        
        #endregion  
    }
}