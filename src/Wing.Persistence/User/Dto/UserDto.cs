using System.ComponentModel.DataAnnotations;

namespace Wing.Persistence.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        [Required]
        public string UserAccount { get; set; }

        [Required]
        public string Password { get; set; }

        public string Dept { get; set; }

        public string Station { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public string Enabled { get; set; } = "Y";

        public string Token { get; set; }
    }
}
