using SampleApp.Sources.democlient;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var of = new OAuthWorkFlow();

            /* For Oauth tokens please un-comment line 1 to line 4 also please plugin your App Id and Secret from https://developer.deere.com app profile in ApiCredentials.cs CLIENT constructor
             * Once access tokens are generated please plug te valus into  ApiCredentials.cs OAuthToken constructor
             * 
             * To run the file download example, please comment out Line 1 to Line 4 and un-comment Line 5
             * To run the file download example, please comment out Line 1 to Line 5 and un-comment Line 6
             */

            of.RetrieveApiCatalogToEstablishOAuthProviderDetails();  //Line 1
            of.GetRequestToken();                                    //Line 2
            of.AuthorizeRequestToken();                              //Line 3
            of.ExchangeRequestTokenForAccessToken();                 //Line 4

            //Download dn = new Download();                          //Line 5
            //dn.retrieveApiCatalog();

            //Upload up = new Upload();                              //Line 6
            //up.retrieveApiCatalog();                               //Line 7
        }
    }
}
