using Wing.Model;
using Wing.Result;

namespace Wing.Persistence.User
{
    public class UserService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public UserService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public Task<UserDto> Login(User user)
        {
            user.Password = new Encrption().ToMd5(user.Password);
            return _fsql.Select<User>()
                 .Where(x => x.UserAccount == user.UserAccount && x.Password == user.Password)
                 .FirstAsync<UserDto>();
        }

        public async Task<int> Add(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            user.CreationTime = DateTime.Now;
            user.Password = new Encrption().ToMd5(user.Password);
            var exists = await _fsql.Select<User>().AnyAsync(x => x.UserAccount == user.UserAccount);
            if (exists)
            {
                throw new AccountExistsException($"登录账号({user.UserAccount})已存在！");
            }

            return await _fsql.Insert(user).ExecuteAffrowsAsync();
        }

        public async Task<int> Update(User user)
        {
            user.Password = new Encrption().ToMd5(user.Password);
            user.ModificationTime = DateTime.Now;
            var exists = await _fsql.Select<User>().AnyAsync(x => x.UserAccount == user.UserAccount && x.Id != user.Id);
            if (exists)
            {
                throw new Exception($"登录账号({user.UserAccount})已存在！");
            }

            return await _fsql.Update<User>(user).ExecuteAffrowsAsync();
        }

        public Task<int> Delete(string id)
        {
            return _fsql.Delete<User>(id).ExecuteAffrowsAsync();
        }

        public async Task<PageResult<List<UserDto>>> List(PageModel<UserSearchDto> dto)
        {
            var result = await _fsql.Select<User>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.UserName), u => u.UserName.Contains(dto.Data.UserName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.UserAccount), u => u.UserAccount.Contains(dto.Data.UserAccount))
                    .OrderByDescending(x => x.CreationTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync<UserDto>();
            return new PageResult<List<UserDto>>
            {
                TotalCount = total,
                Items = result
            };
        }
    }
}
