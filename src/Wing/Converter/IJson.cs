namespace Wing.Converter
{
    public interface IJson
    {
        string Serialize(object value);

        T Deserialize<T>(string value);

        object Deserialize(string value, Type type);

        T DeserializeAnonymousType<T>(string value, T anonymousTypeObject);
    }
}
