using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TIROTAPI.DataAccess.DataModels
{
    public class MachineStatus
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("MachineID")]
        public string MachineID { get; set; }
        [BsonElement("CDateTime")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime CDateTime { get; set; }
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        [BsonElement("SDateTime")]
        public DateTime SDateTime { get; set; }
        [BsonElement("MachineStatus")]
        public string MachineRunningStatus { get; set; }
    }
}
