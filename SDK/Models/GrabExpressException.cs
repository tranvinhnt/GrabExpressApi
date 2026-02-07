using System;

namespace GrabExpressApi.SDK.Models
{

    public class GrabExpressException : Exception
    {
        public int StatusCode { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public GrabExpressException(string message) : base(message) { }

        public GrabExpressException(string message, int statusCode, string? errorCode = null, string? errorMessage = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

}