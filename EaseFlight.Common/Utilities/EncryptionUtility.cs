using System;
using System.Text;

namespace EaseFlight.Common.Utilities
{
    public class EncryptionUtility
    {
        #region Base64 encryption
        public static string Base64Encode(string encodeMe)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(encodeMe);

            return Convert.ToBase64String(encoded);
        }

        public static string Base64Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);

            return Encoding.UTF8.GetString(encoded);
        }
        #endregion

        #region Bcrypt encryption
        public static string BcryptHashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool BcryptCheckPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static string BcryptGenerateSalt(int take)
        {
            return BCrypt.Net.BCrypt.GenerateSalt(take);
        }
        #endregion
    }
}
