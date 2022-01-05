namespace Wing.Convert
{
    public static class StringExtensions
    {
        public static string RemoveStart(this string val, string toRemove)
        {
            return val.StartsWith(toRemove) ? val.Remove(0, toRemove.Length) : val;
        }
    }
}
