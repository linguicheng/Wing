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

        public string 
    }
}
