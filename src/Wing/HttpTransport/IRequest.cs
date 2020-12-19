using System.Threading.Tasks;

namespace Wing.HttpTransport
{
    public interface IRequest
    {
        Task<T> Post<T>(string uri, object data = null);

        Task<T> Put<T>(string uri, object data = null);

        Task<T> Patch<T>(string uri, object data = null);

        Task<T> Get<T>(string uri);

        Task<bool> Delete(string uri);
    }
}
