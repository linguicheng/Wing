using Wing.Persistence.User;
using Wing.Result;
using Wing.UI.Model;

namespace Wing.UI
{
    public static class UserContext
    {
        /// <summary>
        /// 登录前执行
        /// </summary>
        public static Func<UserDto, ApiResult> UserLoginBefore { get; set; }
        /// <summary>
        /// 登录后执行
        /// </summary>
        public static Func<UserDto, ApiResult> UserLoginAfter { get; set; }
    }
}
