using EaseFlight.Common.Constants;
using EaseFlight.Models.EntityModels;
using System.Web;

namespace EaseFlight.Web.WebUtilities
{
    public class SessionUtility
    {
        #region Session Funtions
        private static void RemoveSessionKey(string sessionKey)
        {
            if (HttpContext.Current.Session[sessionKey] != null)
                HttpContext.Current.Session.Remove(sessionKey);
        }

        private static void SetSessionKey(string sessionKey, object value, int timeout)
        {
            // Remove current session
            RemoveSessionKey(sessionKey);

            // Store data to session
            HttpContext.Current.Session.Timeout = timeout;
            HttpContext.Current.Session.Add(sessionKey, value);
        }

        private static ValueType GetSessionValue<ValueType>(string sessionKey, bool needDecodeData = true)
        {
            ValueType value = default(ValueType);

            try
            {
                // Get user data from session
                var data = HttpContext.Current.Session[sessionKey];

                if (data != null)
                    value = (ValueType)data;
            }
            catch
            {
            }

            return value;
        }
        #endregion

        #region Logged user handles Functions
        public static void SetAuthenticationToken(AccountModel userData, int timeout)
        {
            SetSessionKey(Constant.CONST_SESSION_KEY_LOGGED_USER, userData, timeout);
        }

        public static AccountModel GetLoggedUser()
        {
            var loggedUser = GetSessionValue<AccountModel>(Constant.CONST_SESSION_KEY_LOGGED_USER);

            return loggedUser;
        }

        public static void Logout()
        {
            RemoveSessionKey(Constant.CONST_SESSION_KEY_LOGGED_USER);
        }

        public static bool IsSessionAlive()
        {
            return HttpContext.Current.Session[Constant.CONST_SESSION_KEY_LOGGED_USER] != null;
        }
        #endregion
    }
}