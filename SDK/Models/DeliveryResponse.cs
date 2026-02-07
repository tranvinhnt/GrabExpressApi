namespace GrabExpressApi.SDK.Models;

public class DeliveryResponse
{
    public string DeliveryID { get; set; } = string.Empty;
    public string MerchantOrderID { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string? VehicleType { get; set; }
    public Address? Origin { get; set; }
    public Address? Destination { get; set; }
    public ContactPerson? Sender { get; set; }
    public ContactPerson? Recipient { get; set; }
    public List<Package>? Packages { get; set; }
    public Driver? Driver { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? TrackingUrl { get; set; }
}

public class Driver
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? PlateNumber { get; set; }
    public string? PhotoUrl { get; set; }
}
