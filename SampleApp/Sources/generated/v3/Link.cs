using System.Runtime.Serialization;

namespace SampleApp.Sources.generated.v3
{
    [DataContract]
    class Link
    {
        [DataMember]
        internal string rel;
        [DataMember]
        internal string uri;
        [DataMember]
        internal string followable;
    }
}
