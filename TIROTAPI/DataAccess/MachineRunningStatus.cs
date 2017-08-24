using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
//using TIROTAPI.DataAccess.DataModels;
using TIROTLibrary.Business;

namespace TIROTAPI.DataAccess
{
    public class MachineRunningStatus
    {
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _db;
        private string _mgCollName = "MachineRunningStatus";

        public MachineRunningStatus()
        {
            //MongoCredential authmg = MongoCredential.CreateMongoCRCredential("TIROT", "iotadmin", "083d080d");
            //MongoClientSettings mgsetting = new MongoClientSettings();
            //mgsetting.Credentials = new[] { authmg };
            //mgsetting.Server = new MongoServerAddress("SoldevIOT");
            _client = new MongoClient("mongodb://SoldevIOT:27017");
            //_client = new MongoClient("mongodb://127.0.0.1:27017");


            //_client = new MongoClient("mongodb://192.168.1.52:27017");
            
            
            //_client = new MongoClient(mgsetting);

            _server = _client.GetServer();
            _db = _server.GetDatabase("TIROT");
        }

        public MachineStatusLog Create(MachineStatusLog p)
        {
            p.Id = Guid.NewGuid().ToString();
            _db.GetCollection<MachineStatusLog>(_mgCollName).Save(p);
            return p;
        }


        public IEnumerable<MachineStatusLog> GetMachineStatusByDate(DateTime getDate)
        {
            var res = Query<MachineStatusLog>.GTE(p => p.CDateTime, DateTime.SpecifyKind(getDate, DateTimeKind.Local));
            return _db.GetCollection<MachineStatusLog>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("MachineID","CDateTime"));
        }

        public IEnumerable<MachineStatusLog> GetMachineStatusLogByDate(DateTime inDateTime)
        {
            //var onlydate = new DateTime(inDateTime.Year, inDateTime.Month, inDateTime.Day, 8, 0, 0);

            var onlydate = inDateTime;

            var res = Query.And(Query<MachineStatusLog>.GTE(p => p.CDateTime, DateTime.SpecifyKind(onlydate, DateTimeKind.Local))
                      , Query<MachineStatusLog>.LT(p => p.CDateTime, DateTime.SpecifyKind(onlydate.AddDays(1), DateTimeKind.Local)));

            return _db.GetCollection<MachineStatusLog>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("MachineID", "CDateTime"));
        }

        public IEnumerable<MachineStatusLog> GetMachineStatusByIdAndDate(string machineID,DateTime inDateTime)
        {
            //var onlydate = new DateTime(inDateTime.Year, inDateTime.Month, inDateTime.Day, 8, 0, 0);
            var onlydate = inDateTime;

            var res = Query.And(Query<MachineStatusLog>.EQ(p => p.MachineID, machineID)
                     , Query<MachineStatusLog>.GTE(p => p.CDateTime, DateTime.SpecifyKind(onlydate, DateTimeKind.Local))
                     , Query<MachineStatusLog>.LT(p => p.CDateTime, DateTime.SpecifyKind(onlydate.AddDays(1), DateTimeKind.Local)));
           // var testret = _db.GetCollection<MachineStatusLog>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("CDateTime"));
             return _db.GetCollection<MachineStatusLog>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("CDateTime"));
            //return testret;
        }

        public IEnumerable<MachineStatusLog> GetLastMachineStatus(string machineID)
        {
            var res = Query.And(Query<MachineStatusLog>.EQ(p => p.MachineID, machineID));
            // var testret = _db.GetCollection<MachineStatusLog>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("CDateTime"));
            
            return _db.GetCollection<MachineStatusLog>(_mgCollName).Find(res).SetSortOrder(SortBy.Descending("CDateTime")).SetLimit(1);
            //return _db.GetCollection(_mgCollName).FindAs<MachineStatusLog>(res).SetSortOrder(SortBy.Descending("CDateTime")).SetLimit(1);
            //return testret;
        }
    }
}
