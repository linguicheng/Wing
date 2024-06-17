using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wing.Model;
using Wing.Persistence.User;
using Wing.Result;
using Wing.UI.Helper;

namespace Wing.UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<int> Login(User dto)
        {
            dto.Password = new Encrption().ToMd5(dto.Password);
            return _userService.Add(dto);
        }

        [HttpGet]
        public Task<PageResult<List<UserDto>>> List([FromQuery] PageModel<UserSearchDto> dto)
        {
            return _userService.List(dto);
        }

        [HttpPost]
        public Task<int> Add(User dto)
        {
            dto.Password = new Encrption().ToMd5(dto.Password);
            return _userService.Add(dto);
        }

        [HttpPost]
        public Task<int> Update(User dto)
        {
            dto.Password = new Encrption().ToMd5(dto.Password);
            return _userService.Update(dto);
        }

        [HttpPost]
        public Task<int> Delete(string id)
        {
            return _userService.Delete(id);
        }
    }
}
