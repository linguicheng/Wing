using FreeSql.DataAnnotations;

namespace Wing.Persistence.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string UserAccount { get; set; }

        public string Dept { get; set; }

        public string Station { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }
    }
}
