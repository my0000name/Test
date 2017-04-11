using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Linq;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Test
{
    public class MongoTest
    {

        public void f()
        {
            string Map = @"function(){
                                            var value = {
                                                Count:1,
                                                ValidCount: this.QA1_s1 <=10 ? 1:0
                                            };
                                            emit(this.flagname ,value);
                                        }";
            string Reduce = @"function(key,values){
                                            var result = {Count:0,ValidCount:0}
                                            values.forEach(
                                                function(value){
                                                    result.Count += value.Count;
                                                    result.ValidCount += value.ValidCount;
                                                }
                                            );
                                            return result;
                                        }";
            string Finalize = @"function(key, reduced){
	                                        return reduced;
                                        }";

            IMongoDatabase db = Db.GetDb();
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("Data");
            MapReduceOptions<BsonDocument, BsonDocument> o = new MapReduceOptions<BsonDocument, BsonDocument>();
            BsonValue bv = BsonValue.Create("this.Sys_Upload_Time >= ISODate(\"2017-03-01T00:00:00Z\") && this.Sys_Upload_Time<= ISODate(\"2017-04-07T23: 59:59Z\")");
            o.Filter = new BsonDocument("$where", bv);
            o.JavaScriptMode = true;
            o.OutputOptions = MapReduceOutputOptions.Inline;
            o.Finalize = new BsonJavaScript(Finalize);
            IAsyncCursor<BsonDocument> cursor = collection.MapReduce<BsonDocument>(new BsonJavaScript(Map), new BsonJavaScript(Reduce), o);
            JArray array = new JArray();
            if (cursor.MoveNext())
            {
                array = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(cursor.Current.ToJson());
            }
            return;
        }
    }



    public class Db
    {
        private static readonly string connStr = "mongodb://admin:123@10.137.254.220";// ConfigurationManager.ConnectionStrings["connStr"].ToString();

        private static readonly string dbName = "SR_151117102727152001";//ConfigurationManager.AppSettings["dbName"].ToString();

        private static IMongoDatabase db = null;

        private static readonly object lockHelper = new object();

        private Db() { }

        public static IMongoDatabase GetDb()
        {

            var client = new MongoClient(connStr);
            db = client.GetDatabase(dbName);

            return db;
        }

        public static IMongoDatabase GetDb(string dbname)
        {

            var client = new MongoClient(connStr);
            db = client.GetDatabase(dbname);

            return db;
        }
    }

    public class MongoDbHelper
    {
        private IMongoDatabase db = null;

        private IMongoCollection<BsonDocument> collection = null;

        public MongoDbHelper(string databaseName)
        {
            this.db = Db.GetDb(databaseName);
            collection = db.GetCollection<BsonDocument>("Data");
        }

        public void Insert<T>(T entity)
        {

            collection.InsertOne(entity.ToBsonDocument());
        }

        //public void Modify(string id, string field, string value)
        //{
        //    var filter = Builders<T>.Filter.Eq("Id", ObjectId.Parse(id));
        //    var updated = Builders<T>.Update.Set(field, value);
        //    UpdateResult result = collection.UpdateOneAsync(filter, updated).Result;
        //}

        //public void Update(T entity)
        //{
        //    var old = collection.Find(e => e.Id.Equals(entity.Id)).ToList().FirstOrDefault();

        //    foreach (var prop in entity.GetType().GetProperties())
        //    {
        //        var newValue = prop.GetValue(entity);
        //        var oldValue = old.GetType().GetProperty(prop.Name).GetValue(old);
        //        if (newValue != null)
        //        {
        //            if (!newValue.ToString().Equals(oldValue.ToString()))
        //            {
        //                old.GetType().GetProperty(prop.Name).SetValue(old, newValue.ToString());
        //            }
        //        }
        //    }
        //    old.State = "y";
        //    old.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //    var filter = Builders<T>.Filter.Eq("Id", entity.Id);
        //    ReplaceOneResult result = collection.ReplaceOneAsync(filter, old).Result;
        //}

        //public void Delete(T entity)
        //{
        //    var filter = Builders<T>.Filter.Eq("Id", entity.Id);
        //    collection.DeleteOneAsync(filter);
        //}

        //public T QueryOne(string id)
        //{
        //    return collection.Find(a => a.Id == ObjectId.Parse(id)).ToList().FirstOrDefault();
        //}

        //public List<T> QueryAll()
        //{
        //    return collection.Find(a => a.State.Equals("y")).ToList();
        //}
    }






}
