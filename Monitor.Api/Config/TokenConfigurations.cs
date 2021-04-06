namespace Monitor.Api.Config
{
    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int SecondsToExpire { get; set; }
    }
}