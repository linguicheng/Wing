using FreeSql.DataAnnotations;

namespace Wing.Persistence.User
{
    public class User
    {
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string UserAccount { get; set; }

        public string Password { get; set; }

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
    }
}
