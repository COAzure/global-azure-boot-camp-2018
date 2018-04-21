using System;
using System.Runtime.Serialization;

namespace AzureServerless.EventGrid1.Models
{
    [DataContract]
    public class GridEvent
    {
        // Required, useful to identify individual events
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id
        {
            get; set;
        }

        // Required 
        [DataMember(Name = "subject")]
        public string Subject
        {
            get; set;
        }

        // Required
        [DataMember(Name = "data", EmitDefaultValue = false)]
        public object Data
        {
            get; set;
        }

        // Required
        [DataMember(Name = "eventType")]
        public string EventType
        {
            get; set;
        }

        // Required
        [DataMember(Name = "eventTime", EmitDefaultValue = false)]
        public DateTime EventTime
        {
            get; set;
        }
    }
}