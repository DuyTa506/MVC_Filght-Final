namespace EaseFlight.Common.Constants
{
    public partial class Constant
    {
        #region DB Column Names
        public static string CONST_DB_COLUMN_ID = "ID";
        #endregion

        #region DB Value Names
        public static string CONST_DB_NAME_VIETNAM = "Viet Nam";
        public static string CONST_DB_NAME_ADULT = "Adult";
        public static string CONST_DB_NAME_CHILD = "Child";
        public static string CONST_DB_NAME_INFANT = "Infant";
        public static int CONST_DB_SEAT_CLASS_FIRSTCLASS_ID = 1;
        public static int CONST_DB_SEAT_CLASS_BUSINESS_ID = 2;
        public static int CONST_DB_SEAT_CLASS_ECONOMY_ID = 3;
        public static string CONST_PASSWORD_DEFAULT = "P@ssword123";

        #endregion

        #region Session keys
        public static string CONST_SESSION_KEY_LOGGED_USER = "LoggedUser";
        public static string CONST_SESSION_KEY_BOOKING = "Booking";
        public static string CONST_SESSION_KEY_PASSENGER = "Passenger";
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
        public static string CONST_MESSAGE_LOGIN_DISABLE = "Your account has been disabled. Please contact support team EaseFlight!";
        public static string CONST_MESSAGE_USERNAME_LOGIN_WITH_GG_FB = "Your account is binding with Google or Facebook, try to sign in with this method!";
        #endregion

        #region Emails
        public static string CONST_EMAIL_DEFAULT_EMAIL_SENDER = "warning.customer.service@gmail.com";
        public static string CONST_EMAIL_DEFAULT_EMAIL_SENDER_PASSWORD = "tanluc123";
        public static string CONST_EMAIL_RESET_PASSWORD_SUBJECT = "[Ease Flight] Reset your password";
        public static string CONST_EMAIL_RESET_PASSWORD_BODY1 = "<html><body style=\"font-size:13px; font-family:Tahoma;\"><h1>Password reset</h1><p>To reset your password click the link below:</p>"
                                                            + "<div><a href=\"";
        public static string CONST_EMAIL_RESET_PASSWORD_BODY2 = "\">Reset Password</a></div><br /><p>This link will expire in 30 minutes.</p><p>"
                                                            + "If you did not request your password to be reset, please ignore this email and your password will stay as it is.</p><br /></body></html>";
        public static string CONST_EMAIL_RESET_PASSWORD_LINK = @"Account/ResetPassword?rt={0}";
        public static string CONST_EMAIL_BOOK_SUCCESS_SUBJECT = "[Ease Flight] Ticket booking successflly!";
        public static string CONST_EMAIL_RETURN_TICKET_SUBJECT = "[Ease Flight] Return ticket successflly!";


        #endregion

        #region Fight Status
        public static string CONST_FLIGHT_STATUS_READY = "Ready";
        public static string CONST_FLIGHT_STATUS_ONLINE = "Online";
        public static string CONST_FLIGHT_STATUS_DELAY = "Delay";
        public static string CONST_FLIGHT_STATUS_DONE = "Done";
        #endregion

        #region Plane Status
        public static string CONST_PLANE_STATUS_READY = "Ready";
        public static string CONST_PLANE_STATUS_ONLINE = "Online";
        public static string CONST_PLANE_STATUS_REPAIR = "Repair";
        #endregion

        #region Ticket Status
        public static string CONST_DB_TICKET_STATUS_SUCCESS = "Success";
        public static string CONST_DB_TICKET_STATUS_FAILD = "Faild";
        public static string CONST_DB_TICKET_STATUS_RETURN = "Return";
        public static string CONST_DB_TICKET_STATUS_CANCEL = "Cancel";
        #endregion
    }
}
