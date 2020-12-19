using System;

namespace Wing.Exceptions
{
    public class ArgumentEmptyException : ArgumentException
    {
        public ArgumentEmptyException(string paramName)
            : base("Argument were empty", paramName)
        {
        }
    }
}
