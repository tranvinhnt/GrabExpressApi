using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GrabExpressApi.SDK.Models
{

    public class DeliveryQuoteRequest
    {
        public string MerchantOrderID { get; set; } = string.Empty;
        public string ServiceType { get; set; } = "INSTANT";
   
        public string? VehicleType { get; set; }
        public string? CashOnDelivery { get; set; }
        public string? PromoCode { get; set; }
        public Schedule? Schedule { get; set; }
    
        [JsonProperty("codType")]
        public string CodType { get; set; }

        [JsonProperty("packages")]
        public List<Package> Packages { get; set; }

        [JsonProperty("origin")]
        public Destination Origin { get; set; }

        [JsonProperty("destination")]
        public Destination Destination { get; set; }
    }

    public class Schedule
    {
        public DateTime PickupTimeFrom { get; set; }
        public DateTime PickupTimeTo { get; set; }
    }

}