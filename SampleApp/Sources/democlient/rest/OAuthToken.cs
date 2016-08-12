namespace SampleApp.Sources.democlient.rest
{
    class OAuthToken
    {
        public string Token { get; }

        public string Secret { get; }

        public OAuthToken(string token, string secret)
        {
            Token = token;
            Secret = secret;
        }
    }
}
