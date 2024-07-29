namespace Wing.Persistence.User
{
    public class AccountExistsException : Exception
    {
        public AccountExistsException(string message)
            : base(message)
        {
        }
    }
}
