using GrabExpressApi.SDK;
using GrabExpressApi.SDK.Configuration;
using GrabExpressApi.SDK.Models;

namespace GrabExpressApi.Examples;

/// <summary>
/// Example usage of the Grab Express SDK
/// </summary>
public class GrabExpressExamples
{
    public static async Task RunExamplesAsync()
    {
        // Initialize the SDK client
        var config = new GrabExpressConfig
        {
            ClientId = "your-client-id",
            ClientSecret = "your-client-secret",
            Environment = "staging" // or "production"
        };

        using var client = new GrabExpressClient(config);

        // Example 1: Get Delivery Quotes
        await GetQuotesExample(client);

        // Example 2: Create a Delivery
        await CreateDeliveryExample(client);

        // Example 3: Get Delivery Details
        await GetDeliveryDetailsExample(client, "delivery-id-here");

        // Example 4: Cancel a Delivery
        await CancelDeliveryExample(client, "delivery-id-here");

        // Example 5: Submit a Tip
        await SubmitTipExample(client, "delivery-id-here");
    }

    private static async Task GetQuotesExample(GrabExpressClient client)
    {
        try
        {
            var quoteRequest = new DeliveryQuoteRequest
            {
                MerchantOrderID = "ORDER-12345",
                ServiceType = "INSTANT",
                Origin = new Address
                {
                    AddressLine = "123 Main Street, Singapore",
                    Coordinates = new Coordinates
                    {
                        Latitude = 1.3521,
                        Longitude = 103.8198
                    }
                },
                Destination = new Address
                {
                    AddressLine = "456 Orchard Road, Singapore",
                    Coordinates = new Coordinates
                    {
                        Latitude = 1.3048,
                        Longitude = 103.8318
                    }
                },
                Packages = new List<Package>
                {
                    new Package
                    {
                        Name = "Electronics",
                        Description = "Laptop",
                        Quantity = 1,
                        Price = 1500.00m,
                        Dimensions = new Dimensions
                        {
                            Height = 30,
                            Width = 40,
                            Depth = 5,
                            Weight = 2.5
                        }
                    }
                }
            };

            var quotes = await client.GetDeliveryQuotesAsync(quoteRequest);

            Console.WriteLine($"Available quotes ({quotes.Currency}):");
            foreach (var quote in quotes.Quotes)
            {
                Console.WriteLine($"- {quote.ServiceType} ({quote.VehicleType}): {quote.Amount} " +
                                  $"(~{quote.EstimatedTimeline} mins)");
            }
        }
        catch (GrabExpressException ex)
        {
            Console.WriteLine($"Error getting quotes: {ex.Message} (Status: {ex.StatusCode})");
        }
    }

    private static async Task CreateDeliveryExample(GrabExpressClient client)
    {
        try
        {
            var deliveryRequest = new CreateDeliveryRequest
            {
                MerchantOrderID = "ORDER-12345",
                ServiceType = "INSTANT",
                Origin = new Address
                {
                    AddressLine = "123 Main Street, Singapore",
                    Coordinates = new Coordinates
                    {
                        Latitude = 1.3521,
                        Longitude = 103.8198
                    }
                },
                Destination = new Address
                {
                    AddressLine = "456 Orchard Road, Singapore",
                    Coordinates = new Coordinates
                    {
                        Latitude = 1.3048,
                        Longitude = 103.8318
                    }
                },
                Sender = new ContactPerson
                {
                    Name = "John Doe",
                    Phone = "+6591234567",
                    Email = "john@example.com"
                },
                Recipient = new ContactPerson
                {
                    Name = "Jane Smith",
                    Phone = "+6597654321",
                    Email = "jane@example.com"
                },
                Packages = new List<Package>
                {
                    new Package
                    {
                        Name = "Electronics",
                        Description = "Laptop",
                        Quantity = 1,
                        Price = 1500.00m
                    }
                }
            };

            var delivery = await client.CreateDeliveryAsync(deliveryRequest);

            Console.WriteLine($"Delivery created successfully!");
            Console.WriteLine($"Delivery ID: {delivery.DeliveryID}");
            Console.WriteLine($"Status: {delivery.Status}");
            Console.WriteLine($"Tracking URL: {delivery.TrackingUrl}");
        }
        catch (GrabExpressException ex)
        {
            Console.WriteLine($"Error creating delivery: {ex.Message} (Status: {ex.StatusCode})");
        }
    }

    private static async Task GetDeliveryDetailsExample(GrabExpressClient client, string deliveryId)
    {
        try
        {
            var delivery = await client.GetDeliveryDetailsAsync(deliveryId);

            Console.WriteLine($"Delivery Details:");
            Console.WriteLine($"ID: {delivery.DeliveryID}");
            Console.WriteLine($"Status: {delivery.Status}");
            Console.WriteLine($"Service Type: {delivery.ServiceType}");

            if (delivery.Driver != null)
            {
                Console.WriteLine($"Driver: {delivery.Driver.Name} ({delivery.Driver.Phone})");
                Console.WriteLine($"Vehicle: {delivery.Driver.PlateNumber}");
            }
        }
        catch (GrabExpressException ex)
        {
            Console.WriteLine($"Error getting delivery details: {ex.Message} (Status: {ex.StatusCode})");
        }
    }

    private static async Task CancelDeliveryExample(GrabExpressClient client, string deliveryId)
    {
        try
        {
            await client.CancelDeliveryAsync(deliveryId);
            Console.WriteLine($"Delivery {deliveryId} cancelled successfully");
        }
        catch (GrabExpressException ex)
        {
            Console.WriteLine($"Error cancelling delivery: {ex.Message} (Status: {ex.StatusCode})");
        }
    }

    private static async Task SubmitTipExample(GrabExpressClient client, string deliveryId)
    {
        try
        {
            var tipRequest = new SubmitTipRequest
            {
                DeliveryID = deliveryId,
                Amount = 5.00m,
                Currency = "SGD"
            };

            var response = await client.SubmitTipAsync(tipRequest);
            Console.WriteLine($"Tip submitted: {response.Status} - {response.Message}");
        }
        catch (GrabExpressException ex)
        {
            Console.WriteLine($"Error submitting tip: {ex.Message} (Status: {ex.StatusCode})");
        }
    }
}
