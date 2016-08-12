namespace SampleApp.Sources.democlient.rest
{
    class OAuthClient
    {
        public string Key { get; }

        public string Secret { get; }

        public OAuthClient(string key, string secret)
        {
            Key = key;
            Secret = secret;
        }
    }
}
