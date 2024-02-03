using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.User
{
    public class UserService : IUserService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public UserService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public Task<int> Add(User user)
        {
           return _fsql.Insert(user).ExecuteAffrowsAsync();
        }

        public async Task<PageResult<List<UserDto>>> List(PageModel<UserSearchDto> dto)
        {
            var result = await _fsql.Select<User>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.UserName), u => u.UserName.Contains(dto.Data.UserName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.UserAccount), u => u.UserAccount.Contains(dto.Data.UserAccount))
                    .OrderByDescending(x => x.RequestTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync();
            return new PageResult<List<Log>>
            {
                TotalCount = total,
                Items = result
            };
        }
    }
}
