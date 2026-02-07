namespace GrabExpressApi.SDK.Models;

public class DeliveryQuoteRequest
{
    public string MerchantOrderID { get; set; } = string.Empty;
    public string ServiceType { get; set; } = "INSTANT";
    public Address Origin { get; set; } = new();
    public Address Destination { get; set; } = new();
    public List<Package> Packages { get; set; } = new();
    public string? VehicleType { get; set; }
    public string? CashOnDelivery { get; set; }
    public string? PromoCode { get; set; }
    public Schedule? Schedule { get; set; }
}

public class Schedule
{
    public DateTime PickupTimeFrom { get; set; }
    public DateTime PickupTimeTo { get; set; }
}
