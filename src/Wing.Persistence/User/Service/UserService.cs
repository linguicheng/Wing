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

            if (user.Enabled != "Y")
            {
                throw new Exception("当前账号不可用！");
            }

            var errorCount = user.ErrorCount.GetValueOrDefault();
            var lockedTime = user.LockedTime.GetValueOrDefault();
            var leftCount = user.LeftCount == null ? 5 : user.LeftCount;
            var now = DateTime.Now;
            if (leftCount <= 0 && errorCount > 5 && errorCount <= 7 && lockedTime.AddMinutes(1) > now)
            {
                throw new Exception("当前账号已被限制1分钟内不允许登录！");
            }

            if (leftCount <= 0 && errorCount > 7 && errorCount <= 9 && lockedTime.AddMinutes(5) > now)
            {
                throw new Exception("当前账号已被限制5分钟内不允许登录！");
            }

            if (leftCount <= 0 && errorCount > 9 && errorCount <= 11 && lockedTime.AddMinutes(20) > now)
            {
                throw new Exception("当前账号已被限制20分钟内不允许登录！");
            }

            if (leftCount <= 0 && errorCount > 11)
            {
                throw new Exception("当前账号已被永久锁定，请联系管理员解锁！");
            }

            if (errorCount == 6
                || errorCount == 8
                || errorCount == 10)
            {
                leftCount = 2;
            }

            dto.Password = new Encrption().ToMd5(dto.Password);

            if (user.Password != dto.Password)
            {
                await _fsql.Update<User>()
                  .Set(x => x.ErrorCount, errorCount + 1)
                  .Set(x => x.LeftCount, leftCount - 1)
                  .Set(x => x.LockedTime, DateTime.Now)
                  .Where(x => x.UserAccount == dto.UserAccount)
                  .ExecuteAffrowsAsync();
                throw new Exception("您输入的账号或密码错误！");
            }

            await _fsql.Update<User>()
                  .Set(x => x.ErrorCount, 0)
                  .Set(x => x.LeftCount, 3)
                  .Where(x => x.UserAccount == dto.UserAccount)
                  .ExecuteAffrowsAsync();

            return user.Adapt<UserDto>();
        }

        public async Task<int> Add(User user)
        {
            var password = App.Configuration["User:Password"];
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("未配置默认密码！");
            }

            user.Password = new Encrption().ToMd5(password);
            user.Id = Guid.NewGuid().ToString();
            user.CreationTime = DateTime.Now;
            var exists = await _fsql.Select<User>().AnyAsync(x => x.UserAccount == user.UserAccount);
            if (exists)
            {
                throw new AccountExistsException($"登录账号({user.UserAccount})已存在！");
            }

            return await _fsql.Insert(user).ExecuteAffrowsAsync();
        }

        public async Task<int> Update(User user)
        {
            user.ModificationTime = DateTime.Now;
            var exists = await _fsql.Select<User>().AnyAsync(x => x.UserAccount == user.UserAccount && x.Id != user.Id);
            if (exists)
            {
                throw new Exception($"登录账号({user.UserAccount})已存在！");
            }

            return await _fsql.Update<User>()
                .Set(x => x.UserAccount, user.UserAccount)
                .Set(x => x.UserName, user.UserName)
                .Set(x => x.Enabled, user.Enabled)
                .Set(x => x.Dept, user.Dept)
                .Set(x => x.Station, user.Station)
                .Set(x => x.Phone, user.Phone)
                .Set(x => x.ModificationTime, user.ModificationTime)
                .Set(x => x.Remark, user.Remark)
                .Where(x => x.Id == user.Id)
                .ExecuteAffrowsAsync();
        }

        public async Task<int> UpdatePassword(UserUpdatePasswordDto dto)
        {
            if (dto.Password == dto.NewPassword)
            {
                throw new AccountExistsException($"新密码与原密码一致，无需修改！");
            }

            var encrption = new Encrption();
            dto.Password = encrption.ToMd5(dto.Password);
            var exists = await _fsql.Select<User>().AnyAsync(x => x.Id == dto.Id && x.Password == dto.Password);
            if (!exists)
            {
                throw new AccountExistsException($"原密码错误！");
            }

            dto.NewPassword = encrption.ToMd5(dto.NewPassword);
            return await _fsql.Update<User>()
                   .Set(x => x.Password, dto.NewPassword)
                   .Where(x => x.Id == dto.Id)
                   .ExecuteAffrowsAsync();
        }

        public async Task<int> ResetPassword(string id)
        {
            var password = App.Configuration["User:Password"];
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("未配置默认密码！");
            }

            password = new Encrption().ToMd5(password);
            return await _fsql.Update<User>()
                   .Set(x => x.Password, password)
                   .Where(x => x.Id == id)
                   .ExecuteAffrowsAsync();
        }

        public async Task<int> Unlocked(string id)
        {
            return await _fsql.Update<User>()
                   .Set(x => x.ErrorCount, null)
                   .Set(x => x.LeftCount, null)
                   .Set(x => x.LockedTime, null)
                   .Where(x => x.Id == id)
                   .ExecuteAffrowsAsync();
        }

        public async Task<int> UpdateTheme(User user)
        {
            return await _fsql.Update<User>()
                   .Set(x => x.ThemeName, user.ThemeName)
                   .Where(x => x.Id == user.Id)
                   .ExecuteAffrowsAsync();
        }

        public Task<int> Delete(string id)
        {
            return _fsql.Delete<User>(id).ExecuteAffrowsAsync();
        }

        public async Task<PageResult<List<UserListDto>>> List(PageModel<UserSearchDto> dto)
        {
            var result = await _fsql.Select<User>()
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.UserName), u => u.UserName.Contains(dto.Data.UserName))
                    .WhereIf(!string.IsNullOrWhiteSpace(dto.Data.UserAccount), u => u.UserAccount.Contains(dto.Data.UserAccount))
                    .OrderByDescending(x => x.CreationTime)
                    .Count(out var total)
                    .Page(dto.PageIndex, dto.PageSize)
                    .ToListAsync<UserListDto>();
            return new PageResult<List<UserListDto>>
            {
                TotalCount = total,
                Items = result
            };
        }
    }
}
