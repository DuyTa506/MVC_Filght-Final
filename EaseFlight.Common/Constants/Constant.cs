namespace EaseFlight.Common.Constants
{
    public partial class Constant
    {
        #region DB Column Names
        public static string CONST_DB_COLUMN_ID = "ID";
        #endregion

        #region Session keys
        public static string CONST_SESSION_KEY_LOGGED_USER = "LoggedUser";
        #endregion

        #region Roles
        public static string CONST_ROLE_USER = "User";
        public static string CONST_ROLE_ADMIN = "Admin";
        public static string CONST_ROLE_SUPERADMIN = "Super Admin";
        #endregion

        #region Messages
        public static string CONST_MESSAGE_EMAIL_SENT_RESET_PASSWORD_SUCCESS = "Email sent with password reset instructions<br/>(Missing Emails! Have you checked your Spam Folder?)";
        public static string CONST_MESSAGE_USERNAME_OR_EMAIL_INVALID = "Username or email does not exist!";
        public static string CONST_MESSAGE_LOGIN_INVALID = "Invalid username or password!";
        public static string CONST_MESSAGE_USERNAME_LOGIN_WITH_GG_FB = "Your account is binding with Google or Facebook, try to sign in with this method!";
        #endregion

        #region Emails
        public static string CONST_EMAIL_DEFAULT_EMAIL_SENDER = "warning.customer.service@gmail.com";
        public static string CONST_EMAIL_DEFAULT_EMAIL_SENDER_PASSWORD = "tanluc123";
        public static string CONST_EMAIL_RESET_PASSWORD_SUBJECT = "[Ease Flight] Reset your password";
        public static string CONST_EMAIL_RESET_PASSWORD_BODY1 = "<html><body style=\"font-size:13px; font-family:Tahoma;\"><h1>Password reset</h1><p>To reset your password click the link below:</p>"
                                                            + "<div><a href=\"";
        public static string CONST_EMAIL_RESET_PASSWORD_BODY2 = "\">Link</a></div><br /><p>This link will expire in 30 minutes.</p><p>"
                                                            + "If you did not request your password to be reset, please ignore this email and your password will stay as it is.</p><br /></body></html>";
        public static string CONST_EMAIL_RESET_PASSWORD_LINK = @"Account/ResetPassword?rt={0}";

        #endregion
    }
}
