using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M101DotNet
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    MainAsync(args).GetAwaiter().GetResult();
        //    Console.WriteLine();
        //    Console.WriteLine("Press Enter");
        //    Console.ReadLine();
        //}

        //static async Task MainAsync(string[] args)
        //{
        //    var connectionString = "mongodb://localhost:27017";

        //    var client = new MongoClient(connectionString);

        //    var db = client.GetDatabase("students");
        //    var col = db.GetCollection<BsonDocument>("grades");

        //    var filter = new BsonDocument();
        //    var sort = Builders<BsonDocument>.Sort.Ascending("student_id").Ascending("score");

        //    var list = await col.Find(filter).Sort(sort).ToListAsync();

        //    BsonValue previous_id = "";
        //    BsonValue student_id = "";
        //    foreach (var doc in list)
        //    {
        //        student_id = doc["student_id"];
        //        if (student_id != previous_id)
        //        {
        //            previous_id = student_id;
        //            Console.WriteLine(doc);

        //            // Deletes record
        //            //var result = await col.DeleteOneAsync(doc);
        //            var col1 = await col.Find(filter).CountAsync();
        //        }
        ////            print "Removing", doc
        ////grades.remove( { '_id': doc['_id'] } )

        ////        Console.WriteLine(doc);
        //    }
        //}

        //C:\data\hw>"C:\Program Files\MongoDB\Server\3.0\bin\mongoimport" -d students -c grades<grades.json
        //C:\data\hw>"C:\Program Files\MongoDB\Server\3.0\bin\mongorestore" --drop --host localhost dump

        // db.grades.aggregate( { '$group' : { '_id' : '$student_id', 'average' : { $avg 
        // : '$score' }  
        // } }, { '$sort' : { 'average' : -1 } }, { '$limit' : 1 } )

        // 
        
        private static readonly MongoClient _client = new MongoClient();

        private static void Main()
        {
            try
            {
                Do().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }

        private static async Task Do()
        {
            IMongoDatabase database = _client.GetDatabase("school");
            IMongoCollection<Item> collection = database.GetCollection<Item>("students");

            List<Item> t = await collection.Find(new BsonDocument()).ToListAsync();

            foreach (Item item in t)
            {
                ScoreItem lowerst = item.Scores.OrderBy(x => x.Score).First(x => x.Type == "homework");
                item.Scores.Remove(lowerst);
                ReplaceOneResult result = await collection.ReplaceOneAsync(x => x.Id == item.Id, item);
            }
        }

        [BsonIgnoreExtraElements]
        private class Item
        {
            public int Id { get; set; }

            [BsonElement("scores")]
            public List<ScoreItem> Scores { get; set; }

            [BsonElement("name")]
            public string Name { get; set; }

            public override string ToString()
            {
                return string.Format("Id: {0}, Scores: {1}", Id, Scores);
            }
        }

        private sealed class ScoreItem
        {
            [BsonElement("type")]
            public string Type { get; set; }

            [BsonElement("score")]
            public double Score { get; set; }
        }
    }
}
