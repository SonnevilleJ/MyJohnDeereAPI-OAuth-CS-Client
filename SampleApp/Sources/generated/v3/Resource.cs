using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SampleApp.Sources.generated.v3
{
    [DataContract]
    class Resource
    {
        [DataMember]
        internal List<Link> links;
        [DataMember]
        internal string id;
    }
}
