using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Persistence.User;
using Wing.Result;

namespace Wing.UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserDto> Login(UserDto dto)
        {
            var user = await _userService.Login(dto);

            if (UserContext.UserLoginAfter == null)
            {
                throw new Exception("未定义回调方法：UserContext.UserLoginAfter");
            }

            user = UserContext.UserLoginAfter(user);

            if (string.IsNullOrWhiteSpace(user.Token))
            {
                throw new Exception("token不能为空");
            }

            return user;
        }

        [HttpPost]
        public Task<PageResult<List<UserListDto>>> List(PageModel<UserSearchDto> dto)
        {
            return _userService.List(dto);
        }

        [HttpPost]
        public Task<int> Add(User dto)
        {
            return _userService.Add(dto);
        }

        [HttpPost]
        public Task<int> Update(User dto)
        {
            return _userService.Update(dto);
        }

        [HttpPost]
        public Task<int> UpdatePassword(UserUpdatePasswordDto dto)
        {
            return _userService.UpdatePassword(dto);
        }

        [HttpPost]
        public Task<int> ResetPassword(string id)
        {
            return _userService.ResetPassword(id);
        }

        [HttpPost]
        public Task<int> Unlocked(string id)
        {
            return _userService.Unlocked(id);
        }

        [HttpPost]
        public Task<int> UpdateTheme(User dto)
        {
            return _userService.UpdateTheme(dto);
        }

        [HttpPost]
        public Task<int> Delete(string id)
        {
            return _userService.Delete(id);
        }
    }
}
