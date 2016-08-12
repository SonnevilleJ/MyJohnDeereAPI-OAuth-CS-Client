using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SampleApp.Sources.generated.v3
{
    [DataContract]
    class File : Resource
    {
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string name;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string type;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string createdTime;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string modifiedTime;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal long nativeSize;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]    
    internal string source;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal bool transferPending;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]    
    internal string visibleViaShare;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]    
    internal bool shared;
    //[DataMember]
    //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    //internal bool _new;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string status;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string invalidFileReasonText;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string archived;
    [DataMember]
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    internal string success;
    }
}
