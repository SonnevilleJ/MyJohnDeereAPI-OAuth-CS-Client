using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleApp.Sources.generated.v3;

namespace SampleApp.Sources.democlient.rest
{
    class CollectionPageDeserializer
    {
        public CollectionPage<T> Deserialize<T>(string s)
        {
            var objects = new List<T>();
            var links = new List<Link>();
            var total = 0;

            dynamic dynObj = JsonConvert.DeserializeObject(s);
            var jObj = (JObject) dynObj;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jObj.ToString());

            foreach (var item in dict)
            {
                if ("links" == item.Key)
                {
                    links.AddRange(JsonConvert.DeserializeObject<List<Link>>(item.Value.ToString()));
                }
                else if ("total" == item.Key)
                {
                    total = JsonConvert.DeserializeObject<int>(item.Value.ToString());
                }
                else if ("values" == item.Key)
                {
                    objects = JsonConvert.DeserializeObject<List<T>>(item.Value.ToString());
                    Console.WriteLine("done");
                }
            }

            return new CollectionPage<T>(objects,
                GetSelf(links),
                GetNextPage(links),
                GetPreviousPage(links),
                total);
        }

        private Uri GetSelf(List<Link> links)
        {
            return FindLinkUriByRel(links, "self");
        }

        private Uri FindLinkUriByRel(List<Link> links, string rel)
        {
            foreach (var link in links)
            {
                if (rel == link.rel)
                {
                    return new Uri(link.uri);
                }
            }
            return null;
        }

        private Uri GetPreviousPage(List<Link> links)
        {
            return FindLinkUriByRel(links, "previousPage");
        }

        private Uri GetNextPage(List<Link> links)
        {
            return FindLinkUriByRel(links, "nextPage");
        }
    }
}
