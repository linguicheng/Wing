using System.Text;

namespace Wing.Converter
{
    public class JsonContent : StringContent
    {
        public JsonContent(object data)
            : base(App.GetService<IJson>().Serialize(data), Encoding.UTF8, "application/json")
        {
        }
    }
}
