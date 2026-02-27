using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace GrabExpressApi.SDK.Models
{

    public class CreateDeliveryRequest
    {
        public string MerchantOrderID { get; set; } = string.Empty;
        public string ServiceType { get; set; } = "INSTANT";
        public string? VehicleType { get; set; }
       
        public List<Package> Packages { get; set; } = new List<Package>();
        public string? PromoCode { get; set; }
      
        [JsonProperty("codType")]
        public string CodType { get; set; }

        [JsonProperty("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty("highValue")]
        public bool HighValue { get; set; }

        
        [JsonProperty("origin")]
        public Destination Origin { get; set; }

        [JsonProperty("destination")]
        public Destination Destination { get; set; }

        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }

        [JsonProperty("sender")]
        public Recipient Sender { get; set; }

        [JsonProperty("schedule")]
        public Schedule Schedule { get; set; }
        [JsonProperty("payer")]
        public string Payer { get; set; }
        [JsonProperty("cashOnDelivery")]
        public CashOnDelivery cashOnDelivery { get; set; }
        
    }
    public partial class CashOnDelivery
    {
        public decimal amount { get; set; }
    }
        public partial class Recipient
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("smsEnabled")]
        public bool SmsEnabled { get; set; }

        [JsonProperty("companyName", NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyName { get; set; }
        [JsonProperty("instruction")]
        public string instruction { get; set; }

    }

}