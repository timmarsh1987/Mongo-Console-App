using MongoDB.Bson;
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
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            var connectionString = "mongodb://localhost:27017";

            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("students");
            var col = db.GetCollection<BsonDocument>("grades");

            var filter = new BsonDocument();
            var sort = Builders<BsonDocument>.Sort.Ascending("student_id").Ascending("score");

            var list = await col.Find(filter).Sort(sort).ToListAsync();

            BsonValue previous_id = "";
            BsonValue student_id = "";
            foreach (var doc in list)
            {
                student_id = doc["student_id"];
                if (student_id != previous_id)
                {
                    previous_id = student_id;
                    Console.WriteLine(doc);

                    // Deletes record
                    //var result = await col.DeleteOneAsync(doc);
                    var col1 = await col.Find(filter).CountAsync();
                }
        //            print "Removing", doc
        //grades.remove( { '_id': doc['_id'] } )

        //        Console.WriteLine(doc);
            }
        }
    }
}
