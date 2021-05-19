using System;

namespace Infra.Models
{
    public class ErrorCodeException : Exception
    {
        public ErrorCodeException(int errorCode)
            : base(Parameters.ErrorCode.GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }

        public new object Data { get; set; }
    }
}