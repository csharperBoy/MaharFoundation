using System;

namespace Mahar.Common.Utilities
{
    /// <summary>
    /// Lightweight guard helpers for argument validation.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures the provided reference is not null.
        /// </summary>
        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            if (value is null) throw new ArgumentNullException(parameterName);
            return value;
        }

        /// <summary>
        /// Ensures the provided string is not null or empty.
        /// </summary>
        public static string NotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{parameterName} must not be null or empty", parameterName);
            return value;
        }

        /// <summary>
        /// Ensures the provided value satisfies the given validator.
        /// </summary>
        public static T Valid<T>(T value, Func<T, bool> validator, string parameterName)
        {
            if (validator is null) throw new ArgumentNullException(nameof(validator));
            if (!validator(value)) throw new ArgumentException($"{parameterName} is invalid", parameterName);
            return value;
        }
    }
}
