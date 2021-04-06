using System;

namespace Monitor.Api.Models
{
    public class LoginResponse
    {
        public bool Authenticated { get; }
        public string Created { get; }
        public string Expiration { get; }
        public string AccessToken { get; }
        public string Message { get; }

        public LoginResponse(bool authenticated, string created, string expiration, string accessToken, string message)
        {
            Authenticated = authenticated;
            Created = created;
            Expiration = expiration;
            AccessToken = accessToken;
            Message = message;
        }

        public LoginResponse(bool authenticated, string message):this(authenticated, null, null, null, message)
        {            
        }

        public override bool Equals(object obj)
        {
            return obj is LoginResponse other &&
                   Authenticated == other.Authenticated &&
                   Created == other.Created &&
                   Expiration == other.Expiration &&
                   AccessToken == other.AccessToken &&
                   Message == other.Message;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Authenticated, Created, Expiration, AccessToken, Message);
        }
    }
}
