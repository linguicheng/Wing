using System.Security.Claims;

namespace Wing.Auth
{
    public interface IAuth
    {
        string GetToken();

        string GetToken(string name);

        public string GetToken(params Claim[] claims);
    }
}
