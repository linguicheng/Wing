using System.ComponentModel.DataAnnotations;

namespace Wing.Persistence.User
{
    public class UserUpdatePasswordDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "密码必填")]
        public string Password { get; set; }

        [Required(ErrorMessage = "新密码必填")]
        public string NewPassword { get; set; }
    }
}
