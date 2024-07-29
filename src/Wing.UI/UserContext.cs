using Wing.Persistence.User;

namespace Wing.UI
{
    public static class UserContext
    {
        public static Func<User, User> UserLoginBefore { get; set; }

        public static Func<UserDto, UserDto> UserLoginAfter { get; set; }
    }
}
