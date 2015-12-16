namespace GoogleAuthTest
{
    
    /// <summary>
    /// Authentication implementation using Google Authenticator with a Time-Based verification code.
    /// </summary>
    public class Authenticator
    {
        #region Private Fields

        private readonly TimeBasedOneTimePassword _totp = null;

        #endregion
        
        #region Constructor

        public Authenticator()
        {
            _totp = new TimeBasedOneTimePassword();
        }

        #endregion

        #region Public Methods

        public bool IsValid(string secret, string password, int checkAdjacentIntervals = 1)
        {
            if (password == _totp.GetPassword(secret))
                return true;            
            
            // I'm not wild about this, but the wisdom of the iternet suggests doing this
            // in the event that the clocks are out of sync.
            for (int i = 1; i <= checkAdjacentIntervals; i++)
            {
                if (password == _totp.GetPassword(secret, _totp.GetCurrentCounter() + i))
                    return true;

                if (password == _totp.GetPassword(secret, _totp.GetCurrentCounter() - i))
                    return true;
            }            

            return false;
        }

        #endregion
    }
}
