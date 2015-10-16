using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Newtonsoft.Json;

namespace Kinesis
{
    class Program
    {
        static void Main(string[] args)
        {
            var o = new
            {
                Message = "Hello World",
                Author = "David Judd"
            };

            //convert to byte array in prep for adding to stream
            byte[] oByte = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));

            //create config that points to AWS region
            var config = new AmazonKinesisConfig();
            config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;

            //create client that pulls creds from web.config and takes in Kinesis config
            var client = new AmazonKinesisClient(config);

            var sw = Stopwatch.StartNew();
            Stopwatch sw2 = null;

            var tasks = new List<Task<PutRecordResponse>>();

            int count = int.Parse(ConfigurationManager.AppSettings["Count"]);
            Console.WriteLine("Sending {0} records... One at a time...", count);
            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                //System.Threading.Thread.Sleep(10);
                //sw2 = Stopwatch.StartNew();
                //create stream object to add to Kinesis request
                using (MemoryStream ms = new MemoryStream(oByte))
                {
                    //create put request
                    PutRecordRequest requestRecord = new PutRecordRequest();
                    //list name of Kinesis stream
                    requestRecord.StreamName = "shomi_dev";
                    //give partition key that is used to place record in particular shard
                    requestRecord.PartitionKey = i.ToString();
                    //add record as memorystream
                    requestRecord.Data = ms;

                    //PUT the record to Kinesis
                    var task = client.PutRecordAsync(requestRecord);
                    tasks.Add(task);
                   
                }
                //sw2.Stop();
                ///Console.WriteLine("Async latency is {0}", sw2.ElapsedMilliseconds);       
            }
             
            Console.WriteLine("{0} records sent... Waiting for tasks to complete...", count);
            Task.WaitAll(tasks.ToArray(), -1);
            sw.Stop();
            foreach (var t in tasks)
            {
                if (t.Result.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine(t.Result.HttpStatusCode);
                }
            }
            
            double actionsPerSec = (double)count * 1000 / (double)sw.ElapsedMilliseconds;
            Console.WriteLine("{0} requests in {1} ms. {2:0.00} requests/sec.", count, sw.ElapsedMilliseconds, actionsPerSec);

        }
    }
}
