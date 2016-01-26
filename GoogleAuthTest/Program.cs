using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAuthTest
{
    class AuthenticatorTest
    {
        #region Private Fields

        /// <summary>
        /// The shared secret used by the HOTP and TOTP algorithms
        /// when validating a verification code.
        /// </summary>
        private readonly string _sharedSecret = null;

        #endregion

        #region Main Entry Point

        static void Main(string[] args)
        {
            AuthenticatorTest at = new AuthenticatorTest();
            at.Start();
        }

        #endregion

        #region Constructor

        public AuthenticatorTest()
        {            
            // Initialize the shared secret.

            // This demonstrates how to generate a shared secret.
            // Typically, once one is generated, it will be stored in 
            // a database for future use.            
            _sharedSecret = GenerateSharedSecret(); //"pI7uJmILHf";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Where it all begins.
        /// </summary>
        public void Start()
        {
            DisplaySecret();

            string verificationCode = PromptForVerificationCode();

            PerformAuthentication(verificationCode);
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Generates a 10 character string of A-Z, a-z, 0-9
        /// Randomized using the RandomNumberGenerator.
        /// </summary>
        /// <returns></returns>
        private string GenerateSharedSecret()
        {
            byte[] buffer = new byte[9];

            using (RandomNumberGenerator rng = RNGCryptoServiceProvider.Create())
            {
                rng.GetBytes(buffer);
            }

            // Don't need to worry about any = padding from the
            // Base64 encoding, since our input buffer is divisible by 3
            string sharedSecret = Convert.ToBase64String(buffer).Substring(0, 10).Replace('/', '0').Replace('+', '1');

            return sharedSecret;
        }

        /// <summary>
        /// Displays the shared secret to the user using the Base32Encoder.
        /// </summary>
        /// <remarks>This is what is to be entered into the Google Authenticator app as the KEY 
        /// under Manual Account Entry.  Alternatively, a QR code can be generated.</remarks>
        private void DisplaySecret()
        {
            Console.WriteLine("This is the raw, unencoded shared secret:");
            Console.WriteLine();
            Console.WriteLine(_sharedSecret);
            Console.WriteLine();

            Base32Encoder enc = new Base32Encoder();

            string secret = enc.Encode(Encoding.ASCII.GetBytes(_sharedSecret));

            Console.WriteLine("Create a new account in the Google Authenticator app,");
            Console.WriteLine("then enter the following as the time based key:");
            Console.WriteLine();
            Console.WriteLine(secret);
            Console.WriteLine();            
        }

        /// <summary>
        /// Prompts the user to enter the time based code displayed in the Google Authenticator app.
        /// </summary>
        /// <returns></returns>
        private string PromptForVerificationCode()
        {
            Console.WriteLine("Enter your verification code: ");

            string verificationCode = Console.ReadLine();

            return verificationCode;
        }

        /// <summary>
        /// Uses the authenticator to check validity of the entered verification code
        /// using the current shared secret.
        /// </summary>
        /// <param name="verificationCode"></param>
        private void PerformAuthentication(string verificationCode)
        {
            var authenticator = new Authenticator();

            if (authenticator.IsValid(_sharedSecret, verificationCode))
            {
                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("ERROR!");
            }

            Console.ReadLine();
        }

        #endregion
    }
}
