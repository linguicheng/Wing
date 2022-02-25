using System.ComponentModel.DataAnnotations;

namespace Wing.Dashboard.Model
{
    public class ConfigDto
    {
        [Required(ErrorMessage = "配置Key必填")]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
