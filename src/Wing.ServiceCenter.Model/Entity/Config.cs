using FreeSql.DataAnnotations;

namespace Wing.ServiceCenter.Model
{
    [Table(Name = "SC_Config")]
    public class Config
    {
        [Column(IsPrimary = true, StringLength = 100)]
        public string Key { get; set; }

        [Column(StringLength = -1)]
        public string Value { get; set; }
    }
}
