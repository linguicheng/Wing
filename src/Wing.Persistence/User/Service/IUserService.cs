using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.User
{
    public interface IUserService
    {
        Task<int> Add(User user);

        Task<int> Update(User user);

        Task<int> Delete(string id);

        Task<PageResult<List<UserDto>>> List(PageModel<UserSearchDto> dto);
    }
}
