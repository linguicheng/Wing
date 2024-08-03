using FreeSql.DataAnnotations;

namespace Wing.Persistence.User
{
    [Table(Name = "Sys_User")]
    [Index("UK_Account", "UserAccount", true)]
    [Index("IX_Name", "UserName", false)]
    public class User
    {
        [Column(IsPrimary = true, StringLength = 50)]
        public string Id { get; set; }

        [Column(StringLength = 50)]
        public string UserName { get; set; }

        [Column(StringLength = 50)]
        public string UserAccount { get; set; }

        [Column(StringLength = 200)]
        public string Password { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreatedName { get; set; }

        [Column(StringLength = 50)]
        public string CreatedAccount { get; set; }

        public DateTime? ModificationTime { get; set; }

        [Column(StringLength = 50)]
        public string ModifiedName { get; set; }

        [Column(StringLength = 50)]
        public string ModifiedAccount { get; set; }

        [Column(StringLength = 50)]
        public string Dept { get; set; }

        [Column(StringLength = 50)]
        public string Station { get; set; }

        [Column(StringLength = 50)]
        public string Phone { get; set; }

        [Column(StringLength = 1000)]
        public string Remark { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Column(StringLength = 1)]
        public string Enabled { get; set; } = "Y";

        /// <summary>
        /// 密码错误次数
        /// </summary>
        public int? ErrorCount { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime? LockedTime { get; set; }
    }
}
