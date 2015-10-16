using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Newtonsoft.Json;

namespace Kinesis.Web.Controllers
{
    [RoutePrefix("api/broadcast")]
    public class BroadcastController : ApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post()
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
            config.

            //create client that pulls creds from web.config and takes in Kinesis config
            var client = new AmazonKinesisClient(config);

            using (MemoryStream ms = new MemoryStream(oByte))
            {
                //create put request
                PutRecordRequest requestRecord = new PutRecordRequest();
                //list name of Kinesis stream
                requestRecord.StreamName = "shomi_dev";
                //give partition key that is used to place record in particular shard
                requestRecord.PartitionKey = DateTime.Now.Ticks.ToString();
                //add record as memorystream
                requestRecord.Data = ms;

                //PUT the record to Kinesis
                var response = await client.PutRecordAsync(requestRecord);
                return Ok(new
                {
                    seq = response.SequenceNumber
                });
            }
        }
    }
}