using System.Runtime.Serialization;

namespace SampleApp.Sources.generated.v3
{
    [DataContract]
    class User : Resource
    {
        [DataMember] internal string accountName;
        [DataMember] internal string givenName;
        [DataMember] internal string familyName;
        [DataMember] internal string userType;
        [DataMember] internal string company;
    }
}
