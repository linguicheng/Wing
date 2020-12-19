namespace Wing.Convert
{
    public interface IJson
    {
        string Serialize(object value);

        T Deserialize<T>(string value);

        T DeserializeAnonymousType<T>(string value, T anonymousTypeObject);
    }
}
