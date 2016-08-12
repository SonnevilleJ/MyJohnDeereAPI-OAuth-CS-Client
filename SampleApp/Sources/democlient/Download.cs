using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using SampleApp.Sources.democlient.rest;
using SampleApp.Sources.generated.v3;

namespace SampleApp.Sources.democlient
{
    class Download
    {
        private Dictionary<string, Link> _links;
        private CollectionPage<generated.v3.File> _files;
        private string _firstFileSelfUri;
        private string _filename;
        private long _fileSize;

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
            var credentials = OAuthWorkFlow.CreateOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.Token.Token,
                ApiCredentials.Token.Secret, null, null);

            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = "https://sandboxapi.deere.com/platform/"
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            var apiCatalog = Deserialise<ApiCatalog>(response.ContentStream);

            _links = OAuthWorkFlow.LinksFrom(apiCatalog);

            GetFiles();

            RetrieveMetadataForFile();
            DownloadFileInPiecesAndComputeMd5();
        }

        public void GetFiles()
        {
            var credentials = OAuthWorkFlow.CreateOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.Token.Token,
                ApiCredentials.Token.Secret, null, null);

            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = _links["files"].uri
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            var ds = new CollectionPageDeserializer();

            _files = ds.Deserialize<generated.v3.File>(response.Content);
        }

        public void RetrieveMetadataForFile()
        {
            var fileForMetaData = GetValidFile(_files);
            var linksFromFirstFile = OAuthWorkFlow.LinksFrom(fileForMetaData);

            _firstFileSelfUri = linksFromFirstFile["self"].uri;

            var credentials = OAuthWorkFlow.CreateOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.Token.Token,
                ApiCredentials.Token.Secret, null, null);

            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = _firstFileSelfUri
            };

            request.AddHeader("Accept", "application/vnd.deere.axiom.v3+json");
            var response = client.Request(request);

            var firstFileDetails = Deserialise<generated.v3.File>(response.ContentStream);

            _filename = firstFileDetails.name;
            _fileSize = firstFileDetails.nativeSize;
            Debug.WriteLine("File Name:" + _filename + " \n File Size:" + _fileSize);
        }

        private generated.v3.File GetValidFile(CollectionPage<generated.v3.File> files)
        {
            generated.v3.File fileForMetaData = null;
            for (var i = 0; i < files.Page.Count; i++)
            {
                if (files.Page[i].type != "INVALID" && files.Page[i].type != "UNKNOWN")
                {
                    fileForMetaData = files.Page[i];
                    break;
                }
            }
            if (fileForMetaData == null)
            {
                Debug.WriteLine(" No Files to download");
            }
            return fileForMetaData;
        }

        public void DownloadFileInPiecesAndComputeMd5()
        {
            //Max file size for download is 50 MB
            long maxFileSize = 16*1024*1024;
            var end = _fileSize <= maxFileSize ? _fileSize : maxFileSize;
            if (!System.IO.File.Exists("C:\\" + _filename))
            {
                System.IO.File.Create("C:\\" + _filename).Dispose();
            }
            using (Stream output = System.IO.File.OpenWrite("C:\\" + _filename))
                GetChunkFromStartAndRecurse(0, end, _fileSize, output);
        }

        private void GetChunkFromStartAndRecurse(long start, long chunkSize, long fileSize, Stream output)
        {
            if (fileSize <= chunkSize)
            {
                CreateDownloadRequest(start, fileSize, output);
            }
            else
            {
                CreateDownloadRequest(start, chunkSize, output);
                GetChunkFromStartAndRecurse(start + chunkSize, chunkSize, fileSize - chunkSize, output);
            }
        }

        private void CreateDownloadRequest(long start, long bytesToRead, Stream output)
        {
            var credentials = OAuthWorkFlow.CreateOAuthCredentials(OAuthType.ProtectedResource, ApiCredentials.Token.Token,
                ApiCredentials.Token.Secret, null, null);
            var client = new RestClient
            {
                Authority = "",
                Credentials = credentials
            };

            var request = new RestRequest
            {
                Path = _firstFileSelfUri,
                Method = WebMethod.Get
            };
            request.AddHeader("Accept", "application/zip");
            request.AddParameter("offset", "" + start);
            request.AddParameter("size", "" + bytesToRead);
            var response = client.Request(request);
            using (var input = response.ContentStream)
            {
                input.CopyTo(output);
            }
        }

        public static T Deserialise<T>(Stream stream)
        {
            var deserializer = new DataContractJsonSerializer(typeof(T));
            var result = (T) deserializer.ReadObject(stream);
            return result;
        }
    }
}
