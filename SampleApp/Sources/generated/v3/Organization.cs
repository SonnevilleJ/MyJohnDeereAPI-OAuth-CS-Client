using System.Runtime.Serialization;

namespace SampleApp.Sources.generated.v3
{
    [DataContract]
    class Organization : Resource
    {
        [DataMember] internal string name;
        [DataMember] internal string type;
        [DataMember] internal string accountId;
        [DataMember] internal bool member;
    }
}
