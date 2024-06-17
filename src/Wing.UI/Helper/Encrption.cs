using System.Security.Cryptography;
using System.Text;

namespace Wing.UI.Helper
{
    public class Encrption
    {
        public string ToMd5(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder hashString = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashString.Append(hashBytes[i].ToString("X2"));
                }

                return hashString.ToString();
            }
        }
    }
}
