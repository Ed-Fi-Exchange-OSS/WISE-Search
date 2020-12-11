using System;

namespace EDFI.Modules.Search.Extensions
{
    public static class ObjectExtensions
    {
        public static T ErrorIfNull<T>(this T input, string errorMessageFormat, params object[] errorMessageArgs) where T : class
        {
            if (errorMessageFormat == null)
            {
                throw new ArgumentNullException("errorMessageFormat");
            }

            if (input == null)
            {
                throw new NullReferenceException(string.Format(errorMessageFormat, errorMessageArgs));
            }
            return input;
        }

        public static T ErrorIfNull<T>(this T? input, string errorMessageFormat, params object[] errorMessageArgs) where T : struct
        {
            if (errorMessageFormat == null)
            {
                throw new ArgumentNullException("errorMessageFormat");
            }

            if (input == null)
            {
                throw new NullReferenceException(string.Format(errorMessageFormat, errorMessageArgs));
            }
            return input.Value;
        }

        public static T To<T>(this object obj)
        {
            return (T)obj;
        }
    }
}