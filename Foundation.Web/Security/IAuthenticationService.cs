namespace Foundation.Web.Security
{
    public interface IAuthenticationService
    {
        int PasswordExpiryDays { get; set; }
        int MaximumPasswordAttemptsLimit { get; set; }
        IUser GetUser(string userName);
        SignInResult SignIn(string userName, string password, bool rememberMe = false);
        void SignOut();
    }
}