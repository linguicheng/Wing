using Mapster;
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

        public async Task<UserDto> Login(UserDto dto)
        {
            var user = await _fsql.Select<User>()
                .Where(x => x.UserAccount == dto.UserAccount)
                .FirstAsync();
            if (user == null)
            {
                throw new Exception("您输入的账号或密码错误！");
            }

            var errorCount = user.ErrorCount.GetValueOrDefault();
            var lockedTime = user.LockedTime.GetValueOrDefault();
            var now = DateTime.Now;
            if (errorCount > 3 && errorCount <= 5 && lockedTime.AddMinutes(1) > now)
            {
                throw new Exception("当前账号已被限制1分钟内不允许登录！");
            }

            if (errorCount > 5 && errorCount <= 7 && lockedTime.AddMinutes(20) > now)
            {
                throw new Exception("当前账号已被限制20分钟内不允许登录！");
            }

            if (errorCount > 7)
            {
                throw new Exception("当前账号已被永久锁定，请联系管理员解锁！");
            }

            dto.Password = new Encrption().ToMd5(dto.Password);

            if (user.Password != dto.Password)
            {
                await _fsql.Update<User>()
                  .Set(x => x.ErrorCount, errorCount + 1)
                  .Set(x => x.LockedTime, DateTime.Now)
                  .ExecuteAffrowsAsync();
                throw new Exception("您输入的账号或密码错误！");
            }

            return user.Adapt<UserDto>();
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
