using System.Collections.Generic;

namespace GrabExpressApi.SDK.Models
{

    public class DeliveryQuoteResponse
    {
        public List<Quote> Quotes { get; set; } = new List<Quote>();
        public string Currency { get; set; } = string.Empty;
    }

    public class Quote
    {
        public string ServiceType { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int EstimatedTimeline { get; set; }
        public decimal? Distance { get; set; }
    }

}