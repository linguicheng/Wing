using System.Security.Claims;

namespace Wing.UI.Auth
{
    public interface IAuth
    {
        string GetToken();

        string GetToken(string name);

        public string GetToken(params Claim[] claims);
    }
}
