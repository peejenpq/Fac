using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using TIROTAPI.DataAccess.DataModels;
using Microsoft.Extensions.Configuration;

namespace TIROTAPI.DataAccess
{
    public class IotLoggerMongo
    {
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _db;
        //MongoCredential _authmg;
        private string _mgCollName = "IoTLogger";

        public IotLoggerMongo()
        {
            try
            {
                //MongoCredential authmg = MongoCredential.CreateMongoCRCredential("TIROT", "iotadmin", "083d080d");
                //MongoClientSettings mgsetting = new MongoClientSettings();
                //mgsetting.Credentials = new[] { authmg };
                //mgsetting.Server = new MongoServerAddress("SoldevIOT");
                _client = new MongoClient("mongodb://SoldevIOT:27017");
                //_client = new MongoClient("mongodb://127.0.0.1:27017");


                //_client = new MongoClient("mongodb://192.168.1.52:27017");
                
                
                //_client = new MongoClient(mgsetting);
                //var testdb = IConfiguration.GetConnectionString("DefaultConnection");
                _server = _client.GetServer();
                _db = _server.GetDatabase("TIROT");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IEnumerable<MGLogger> GetProducts()
        {
            return _db.GetCollection<MGLogger>(_mgCollName).FindAll();
        }


        public MGLogger GetProduct(string id)
        {
            var res = Query<MGLogger>.EQ(p => p.Id, id);
            return _db.GetCollection<MGLogger>(_mgCollName).FindOne(res);
        }

        public MGLogger Create(MGLogger p)
        {
            p.Id = Guid.NewGuid().ToString();
            _db.GetCollection<MGLogger>(_mgCollName).Save(p);
            return p;
        }

        public void Update(string id, MGLogger p)
        {
            p.Id = id;
            var res = Query<MGLogger>.EQ(pd => pd.Id, id);
            var operation = Update<MGLogger>.Replace(p);
            _db.GetCollection<MGLogger>(_mgCollName).Update(res, operation);
        }
        public void Remove(string id)
        {
            var res = Query<MGLogger>.EQ(e => e.Id, id);
            var operation = _db.GetCollection<MGLogger>(_mgCollName).Remove(res);
        }

        public IEnumerable<MGLogger> GetLogByDate(DateTime getDate)
        {
            var res = Query<MGLogger>.GTE(p => p.CDateTime, DateTime.SpecifyKind(getDate, DateTimeKind.Local));
            return _db.GetCollection<MGLogger>(_mgCollName).Find(res);
        }
        public IEnumerable<MGLogger> GetLogByDate(DateTime getStartDate, DateTime getEndDate)
        {

            var res = Query.And(Query<MGLogger>.GTE(p => p.CDateTime, DateTime.SpecifyKind(getStartDate, DateTimeKind.Local))
                     , Query<MGLogger>.LTE(p => p.CDateTime, DateTime.SpecifyKind(getEndDate, DateTimeKind.Local)));

            return _db.GetCollection<MGLogger>(_mgCollName).Find(res);
        }



        public IEnumerable<MGLogger> GetYesterdayLog()
        {
            //IEnumerable<MGLogger> resdata;

            var coldata = _db.GetCollection<MGLogger>(_mgCollName);
            var resdata = coldata.Find(Query<MGLogger>.GTE(p => p.SDateTime, DateTime.UtcNow.AddDays(-1)));

            //foreach (var dox in resdata)
            //{
            //    Console.WriteLine(dox);
            //}

            //resdata.SetLimit(100);

            //    Query<MGLogger>.GTE(p => p.SDateTime, DateTime.UtcNow.AddDays(-5)));
            return resdata;
            //var res = Query<MGLogger>.GTE(p => p.SDateTime, DateTime.SpecifyKind(getDate, DateTimeKind.Utc));
            //return _db.GetCollection<MGLogger>(_mgCollName).Find(res);
        }

        public IEnumerable<MGLogger> GetLogByMachineAndDate(string machineID, DateTime inDateTime)
        {
            var onlydate = new DateTime(inDateTime.Year, inDateTime.Month, inDateTime.Day, 0, 0, 0);

            var res = Query.And(Query<MGLogger>.EQ(p => p.MachineID, machineID)
                     , Query<MGLogger>.GTE(p => p.CDateTime, DateTime.SpecifyKind(onlydate, DateTimeKind.Local))
                     , Query<MGLogger>.LT(p => p.CDateTime, DateTime.SpecifyKind(onlydate.AddDays(1), DateTimeKind.Local)));

            return _db.GetCollection<MGLogger>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("MachineID","CDateTime"));
        }

        public IEnumerable<MGLogger> GetLogByMachinePortAndDate(string machineID, string portNo, DateTime inDateTime)
        {
            var onlydate = new DateTime(inDateTime.Year, inDateTime.Month, inDateTime.Day, 0, 0, 0);

            var res = Query.And(Query<MGLogger>.EQ(p => p.MachineID, machineID)
                     , Query<MGLogger>.EQ(p => p.Port, portNo)
                     , Query<MGLogger>.GTE(p => p.CDateTime, DateTime.SpecifyKind(onlydate, DateTimeKind.Local))
                     , Query<MGLogger>.LT(p => p.CDateTime, DateTime.SpecifyKind(onlydate.AddDays(1), DateTimeKind.Local)));

            return _db.GetCollection<MGLogger>(_mgCollName).Find(res).SetSortOrder(SortBy.Ascending("MachineID", "Port", "CDateTime"));
        }
    }
}
