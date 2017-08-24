using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TIROTAPI.DataAccess.DataModels
{
    public class MGLogger
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("MAC")]
        public string MAC { get; set; }
        [BsonElement("UID")]
        public string UID { get; set; }
        [BsonElement("CDateTime")]
        [BsonDateTimeOptions(Kind =System.DateTimeKind.Local)]
        public DateTime CDateTime { get; set; }
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        [BsonElement("SDateTime")]
        public DateTime SDateTime { get; set; }
        [BsonElement("MachineID")]
        public string MachineID { get; set; }
        [BsonElement("Port")]
        public string Port { get; set; }
        [BsonElement("SensorValue")]
        public string SensorValue { get; set; }
    }
}
