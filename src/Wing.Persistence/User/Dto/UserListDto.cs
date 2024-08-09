namespace Wing.Persistence.User
{
    public class UserListDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string UserAccount { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreatedName { get; set; }

        public string CreatedAccount { get; set; }

        public DateTime? ModificationTime { get; set; }

        public string ModifiedName { get; set; }

        public string ModifiedAccount { get; set; }

        public string Dept { get; set; }

        public string Station { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public string Enabled { get; set; } = "Y";

        /// <summary>
        /// 密码错误次数
        /// </summary>
        public int? ErrorCount { get; set; }

        /// <summary>
        /// 剩余容错次数
        /// </summary>
        public int? LeftCount { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime? LockedTime { get; set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        public string ThemeName { get; set; }
    }
}
