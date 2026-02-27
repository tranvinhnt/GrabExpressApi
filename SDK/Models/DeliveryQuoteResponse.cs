using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace GrabExpressApi.SDK.Models
{

    public class DeliveryQuoteResponse
    {
        public Quote[] Quotes { get; set; }

        [JsonProperty("packages")]
        public Package[] Packages { get; set; }

        [JsonProperty("origin")]
        public Destination Origin { get; set; }

        [JsonProperty("destination")]
        public Destination Destination { get; set; }
    }
    public partial class Currency
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
    public class Quote
    {
        [JsonProperty("service")]
        public Service Service { get; set; }

        [JsonProperty("currency")]
        public Currency Currency { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }


        [JsonProperty("distance")]
        public long distance { get; set; }

        [JsonProperty("estimatedTimeline")]
        public EstimatedTimeline EstimatedTimeline { get; set; }
    }
    public class EstimatedTimeline
    {
        public string pickup { get; set; }
        public string dropoff { get; set; }
        public string completed { get; set; }  

    }
    public partial class Destination
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("address_L1")]
        public string address_L1 { get; set; }

        [JsonProperty("address_L2")]
        public string address_L2 { get; set; }


        [JsonProperty("address_L3")]
        public string address_L3 { get; set; }


        [JsonProperty("cityCode")]
        public string CityCode { get; set; }
        [JsonProperty("keywords")]
        public string keywords { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
    }

  
    public partial class Service
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

}