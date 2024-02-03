using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.User
{
    public interface IUserService
    {
        Task<int> Add(User user);

        Task<PageResult<List<UserDto>>> List(PageModel<UserSearchDto> dto);
    }
}
