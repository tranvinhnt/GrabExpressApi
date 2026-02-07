using System.Collections.Generic;

namespace GrabExpressApi.SDK.Models
{

    public class CreateDeliveryRequest
    {
        public string MerchantOrderID { get; set; } = string.Empty;
        public string ServiceType { get; set; } = "INSTANT";
        public string? VehicleType { get; set; }
        public string? CashOnDelivery { get; set; }
        public Address Origin { get; set; } = new Address();
        public Address Destination { get; set; } = new Address();
        public ContactPerson Sender { get; set; } = new ContactPerson();
        public ContactPerson Recipient { get; set; } = new ContactPerson();
        public List<Package> Packages { get; set; } = new List<Package>();
        public string? PaymentMethod { get; set; }
        public string? PromoCode { get; set; }
        public Schedule? Schedule { get; set; }
    }

}