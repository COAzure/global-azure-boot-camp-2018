using AzureServerless.EventGrid1.Models;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureServerless.EventGrid1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await SendToEventGrid();

            await SendToEventGridWithSdk();
        }

        private static async Task SendToEventGridWithSdk()
        {
            try
            {
                string topicKey = ConfigurationManager.AppSettings["eventGridTopicKey"];
                string topicHostname = ConfigurationManager.AppSettings["eventGridTopicHostName"];

                TopicCredentials credentials = new TopicCredentials(topicKey);
                EventGridClient client = new EventGridClient(credentials);

                IList<EventGridEvent> events = new List<EventGridEvent>
                {
                    // NOTE: Anonymous types don't get sent through when using the SDK; serialization problem.
                    //new EventGridEvent
                    //{
                    //    EventTime = DateTime.UtcNow,
                    //    Id = Guid.NewGuid().ToString(),
                    //    Subject = "customer/Contoso",
                    //    DataVersion = "1.0",
                    //    EventType = "CustomerCreated",
                    //    Data = new {TenantId = "112345", ContactId = "foo34343"}
                    //},
                    new EventGridEvent
                    {
                        EventTime = DateTime.UtcNow,
                        Id = Guid.NewGuid().ToString(),
                        Subject = "customer/Fabrikam",
                        DataVersion = "1.0",
                        EventType = "CustomerStarted",
                        Data = new MyData{FirstName = "Michael", LastName = "Collier"}
                    }
                };

                await client.PublishEventsAsync(topicHostname, events);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected static async Task SendToEventGrid()
        {
            string endpoint = ConfigurationManager.AppSettings["eventGridTopicHostName"];
            string sharedAccessKey = ConfigurationManager.AppSettings["eventGridTopicKey"];
            string publisherAddress = $"https://{endpoint}:443/api/events";

            var myEventList = new List<GridEvent>
            {
                new GridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = $"customer/Contoso",
                    EventType = "CustomerCreated",
                    EventTime = DateTime.UtcNow,
                    Data = new{TenantId="123455", ContactId="foo232342"}
                },
                new GridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = $"customer/Fabrikam",
                    EventType = "CustomerPaid",
                    EventTime = DateTime.UtcNow,
                    Data = new{TenantId="123455", ContactId="foo232342"}
                },
                new GridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = $"customer/FooBar",
                    EventType = "CustomerCreated",
                    EventTime = DateTime.UtcNow,
                    Data = new{TenantId="123455", ContactId="foo232342"}
                },

            };

            using (var httpContent = new StringContent(JsonConvert.SerializeObject(myEventList)))
            {
                httpContent.Headers.Add("aeg-sas-key", sharedAccessKey);
                using (var httpClient = new HttpClient())
                {
                    using (var httpResponseMessage = await httpClient.PostAsync(publisherAddress, httpContent))
                    {
                        Console.WriteLine($"Reponse status code: {httpResponseMessage.StatusCode}");
                    }
                }
            }
        }
    }
}
