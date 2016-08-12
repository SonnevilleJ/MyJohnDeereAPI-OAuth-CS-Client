using SampleApp.Sources.democlient.rest;

namespace SampleApp.Sources.democlient
{
    abstract class ApiCredentials
    {
        public static OAuthClient Client = new OAuthClient("your app id from developer.deere.com", "your app secret from developer.deere.com");
        public static OAuthToken Token = new OAuthToken("token generated after running the oauth worflow code", "secret generated after running the oauth workflow code");
    }
}
