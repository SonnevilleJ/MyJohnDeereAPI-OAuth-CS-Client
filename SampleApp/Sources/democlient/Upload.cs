using System.Collections.Generic;
using System.Text;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Newtonsoft.Json;
using SampleApp.Sources.democlient.rest;
using SampleApp.Sources.generated.v3;

namespace SampleApp.Sources.democlient
{
    class Upload
    {
        private Dictionary<string, Link> _links;
        private string _userOrganizations;
        private Link _fileUploadLink;
        private string _newFileLocation;

        private RestClient GetRestClient()
        {
            var credentials = OAuthWorkFlow.CreateOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.Token.Token,
                ApiCredentials.Token.Secret, null, null);

            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };
            return client;
        }

        public void RetrieveApiCatalog()
        {
            var client = GetRestClient();

            var request = new RestRequest
            {
                Path = "https://sandboxapi.deere.com/platform/"
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            var apiCatalog = Download.Deserialise<ApiCatalog>(response.ContentStream);

            _links = OAuthWorkFlow.LinksFrom(apiCatalog);

            GetCurrentUser();
            GetUserOrganizations();
            AddFile();
            UploadFile();
        }

        public void GetCurrentUser()
        {
            var client = GetRestClient();

            var request = new RestRequest
            {
                Path = _links["currentUser"].uri
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            Resource currentUser = Download.Deserialise<User>(response.ContentStream);

            _userOrganizations = OAuthWorkFlow.LinksFrom(currentUser)["organizations"].uri;
        }

        public void GetUserOrganizations()
        {
            var client = GetRestClient();

            var request = new RestRequest
            {
                Path = _userOrganizations
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            var ds = new CollectionPageDeserializer();

            var organizations = ds.Deserialize<Organization>(response.Content);

            var linksFromFirst = OAuthWorkFlow.LinksFrom(organizations.Page[0]);

            _fileUploadLink = linksFromFirst["uploadFile"];
        }

        public void AddFile()
        {
            var apiFile = new File {name = "sampleFile.zip"};

            var client = GetRestClient();

            var request = new RestRequest
            {
                Path = _fileUploadLink.uri,
                Method = WebMethod.Post
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            request.AddHeader("Content-Type", "application/vnd.deere.axiom.v3+json");
            request.AddPostContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(apiFile)));
            var response = client.Request(request);

            _newFileLocation = response.Headers["Location"];
        }

        public void UploadFile()
        {
            var client = GetRestClient();

            var request = new RestRequest
            {
                Path = _newFileLocation,
                Method = WebMethod.Put
            };
            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddPostContent(System.IO.File.ReadAllBytes("C:\\sampleFile.zip"));
            var response = client.Request(request);
        }
    }
}
