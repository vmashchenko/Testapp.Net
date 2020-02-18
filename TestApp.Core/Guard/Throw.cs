using System;

namespace TestApp.Core.Guard
{
    public static class Throw
    {
        public static void IfNull(Object obj, string parameterName)
        {
            CheckParameterValue(parameterName);
            string reason = string.Format("{0} cannot be null", parameterName);
            IfNull(obj, parameterName, reason);
        }

        public static void IfNull(Object obj, string parameterName, string reason)
        {
            CheckParameterValue(parameterName);
            CheckParameterValue(reason);
            ThrowImpl((input) => input == null, obj, new ArgumentNullException(parameterName, reason));
        }

        public static void IfNullOrWhiteSpace(string str, string parameterName)
        {
            CheckParameterValue(parameterName);
            string reason = string.Format("{0} cannot be null or empty", parameterName);
            IFNullOrWhiteSpace(str, parameterName, reason);
        }

        public static void IFNullOrWhiteSpace(string str, string parameterName, string reason)
        {
            IfNull(str, parameterName, reason);
            ThrowImpl((obj) => string.IsNullOrWhiteSpace(obj.ToString()), str, new ArgumentNullException(parameterName, reason));
        }

        public static void IfNegativeOrZero(long number, string parameterName)
        {
            ThrowImpl((obj) => number <= 0, number, new ArgumentOutOfRangeException(parameterName, number, "Cannot be less or equal zero"));
        }

        public static void IfNegativeOrZero(int number, string parameterName)
        {
            ThrowImpl((obj) => number <= 0, number, new ArgumentOutOfRangeException(parameterName, number, "Cannot be less or equal zero"));
        }

        public static void IfZero(long number, string parameterName)
        {
            ThrowImpl((obj) => number == 0, number, new ArgumentOutOfRangeException(parameterName, number, "Cannot be zero"));
        }

        public static void IfZero(int number, string parameterName)
        {
            ThrowImpl((obj) => number == 0, number, new ArgumentOutOfRangeException(parameterName, number, "Cannot be zero"));
        }

        public static void IfFalse(bool value, string parameterName, string reason = null)
        {
            CheckParameterValue(parameterName);
            IfFalse(value, new ArgumentException(reason ?? "Value cannot be false", parameterName));
        }

        public static void IfFalse(bool value, Exception ex)
        {
            ThrowImpl(!value, ex);
        }

        public static void IfTrue(bool value, string parameterName, string reason = null)
        {
            CheckParameterValue(parameterName);
            IfTrue(value, new ArgumentException(reason ?? "Value cannot be true", parameterName));
        }

        public static void IfTrue(bool value, Exception ex)
        {
            ThrowImpl(value, ex);
        }

        private static void CheckParameterValue(string parameter)
        {
            if (parameter == null)
            {
                throw new ThrowException("Input parameter has null value");
            }
        }

        private static void ThrowImpl(Func<object, bool> needToThrow, object obj, Exception ex)
        {
            bool isThrowEx = needToThrow(obj);

            if (!isThrowEx)
            {
                return;
            }

            if (ex == null)
            {
                throw new ThrowException("Exception is not provided");
            }

            throw ex;
        }

        private static void ThrowImpl(bool needToThrow, Exception ex)
        {
            ThrowImpl((obj) => { return bool.Parse(obj.ToString()) == needToThrow; }, needToThrow, ex);
        }
    }
}
