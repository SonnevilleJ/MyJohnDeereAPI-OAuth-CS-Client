using System;
using System.Collections.Generic;
using System.Compat.Web;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using Hammock;
using Hammock.Authentication.OAuth;
using SampleApp.Sources.generated.v3;

namespace SampleApp.Sources.democlient
{
    class OAuthWorkFlow
    {
        private string _authUri;
        private string _verifier;
        Dictionary<string, Link> _links;
        string _reqToken;
        string _reqSecret;

        public void RetrieveApiCatalogToEstablishOAuthProviderDetails()
        {
            var credentials = CreateOAuthCredentials(OAuthType.ProtectedResource, null, null, null, null);

            var client = new RestClient
            {
                Authority = "https://sandboxapi.deere.com/platform/",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = ""
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            var stream1 = new MemoryStream();
            var ser = new DataContractJsonSerializer(typeof(ApiCatalog));

            stream1.Position = 0;
            var apiCatalog = (ApiCatalog) ser.ReadObject(response.ContentStream);

            _links = LinksFrom(apiCatalog);
        }

        public static Dictionary<string, Link> LinksFrom(Resource res)
        {
            var map = new Dictionary<string, Link>();

            foreach (var link in res.links)
            {
                map.Add(link.rel, link);
            }
            return map;
        }

        public void GetRequestToken()
        {
            var credentials = CreateOAuthCredentials(OAuthType.RequestToken, null, null, null, "oob");

            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = _links["oauthRequestToken"].uri
            };

            var response = client.Request(request);

            _reqToken = response.Content.Split('&')[0];

            _authUri = CleanAuthorizationUri(_links["oauthAuthorizeRequestToken"].uri) + "?" + _reqToken;
            _reqToken = _reqToken.Split('=')[1];
            _reqSecret = response.Content.Split('&')[1].Split('=')[1];
        }

        public void AuthorizeRequestToken()
        {
            Console.WriteLine("Please provide the verifier from " + _authUri);
            _verifier = Console.ReadLine();
        }

        public void ExchangeRequestTokenForAccessToken()
        {
            var credentials = CreateOAuthCredentials(OAuthType.AccessToken, _reqToken, HttpUtility.UrlDecode(_reqSecret), _verifier, null);

            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = _links["oauthAccessToken"].uri
            };

            var response = client.Request(request);

            Console.WriteLine("Token:" + response.Content.Split('&')[0].Split('=')[1] + " \n Token Secret:" + response.Content.Split('&')[1].Split('=')[1]);
            var oauthToken = response.Content.Split('&')[0].Split('=')[1];
            var oauthTokenSecret = response.Content.Split('&')[1].Split('=')[1];

            Debug.WriteLine("Token:" + oauthToken + " \n Token Secret:" + HttpUtility.UrlDecode(oauthTokenSecret));
        }

        private static string CleanAuthorizationUri(string uri)
        {
            return uri.Substring(0, uri.IndexOf("?"));
        }

        public static OAuthCredentials CreateOAuthCredentials(OAuthType type, string strToken, string strSecret, string strVerifier, string strCallBack)
        {
            var credentials = new OAuthCredentials
            {
                Type = type,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = ApiCredentials.Client.Key,
                ConsumerSecret = ApiCredentials.Client.Secret,
                Token = strToken,
                TokenSecret = strSecret,
                Verifier = strVerifier,
                CallbackUrl = strCallBack
            };
            return credentials;
        }
    }
}
