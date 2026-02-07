# Grab Express API SDK

A comprehensive .NET SDK for integrating with the Grab Express API. This SDK provides easy-to-use methods for all Grab Express endpoints including delivery quotes, creating deliveries, tracking, cancellation, and tipping.

## Features

- ✅ **Complete API Coverage**: All Grab Express API endpoints wrapped
- ✅ **OAuth 2.0 Authentication**: Automatic token management and refresh
- ✅ **Type-Safe Models**: Strongly-typed request/response models
- ✅ **Error Handling**: Custom exception types with detailed error information
- ✅ **Environment Support**: Easy switching between staging and production
- ✅ **Async/Await**: Modern async programming patterns
- ✅ **Thread-Safe**: Safe for concurrent use

## Installation

Add the SDK to your project by including the SDK folder in your solution.

## Quick Start

### 1. Initialize the Client

```csharp
using GrabExpressApi.SDK;
using GrabExpressApi.SDK.Configuration;

var config = new GrabExpressConfig
{
    ClientId = "your-client-id",
    ClientSecret = "your-client-secret",
    Environment = "staging" // or "production"
};

using var client = new GrabExpressClient(config);
```

### 2. Get Delivery Quotes

```csharp
var quoteRequest = new DeliveryQuoteRequest
{
    MerchantOrderID = "ORDER-12345",
    ServiceType = "INSTANT",
    Origin = new Address
    {
        AddressLine = "123 Main Street, Singapore",
        Coordinates = new Coordinates { Latitude = 1.3521, Longitude = 103.8198 }
    },
    Destination = new Address
    {
        AddressLine = "456 Orchard Road, Singapore",
        Coordinates = new Coordinates { Latitude = 1.3048, Longitude = 103.8318 }
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

var quotes = await client.GetDeliveryQuotesAsync(quoteRequest);
```

### 3. Create a Delivery

```csharp
var deliveryRequest = new CreateDeliveryRequest
{
    MerchantOrderID = "ORDER-12345",
    ServiceType = "INSTANT",
    Origin = new Address { /* ... */ },
    Destination = new Address { /* ... */ },
    Sender = new ContactPerson
    {
        Name = "John Doe",
        Phone = "+6591234567",
        Email = "john@example.com"
    },
    Recipient = new ContactPerson
    {
        Name = "Jane Smith",
        Phone = "+6597654321"
    },
    Packages = new List<Package> { /* ... */ }
};

var delivery = await client.CreateDeliveryAsync(deliveryRequest);
Console.WriteLine($"Delivery ID: {delivery.DeliveryID}");
```

### 4. Get Delivery Details

```csharp
var delivery = await client.GetDeliveryDetailsAsync("delivery-id");
Console.WriteLine($"Status: {delivery.Status}");
Console.WriteLine($"Driver: {delivery.Driver?.Name}");
```

### 5. Cancel a Delivery

```csharp
// Cancel by delivery ID
await client.CancelDeliveryAsync("delivery-id");

// Or cancel by merchant order ID
await client.CancelDeliveryByMerchantOrderIdAsync("ORDER-12345");
```

### 6. Submit a Tip

```csharp
var tipRequest = new SubmitTipRequest
{
    DeliveryID = "delivery-id",
    Amount = 5.00m,
    Currency = "SGD"
};

var response = await client.SubmitTipAsync(tipRequest);
```

## API Methods

| Method | Description |
|--------|-------------|
| `GetDeliveryQuotesAsync()` | Get pricing quotes before placing an order |
| `CreateDeliveryAsync()` | Create a new delivery request |
| `GetDeliveryDetailsAsync()` | Get details of a specific delivery |
| `CancelDeliveryAsync()` | Cancel a delivery by delivery ID |
| `CancelDeliveryByMerchantOrderIdAsync()` | Cancel deliveries by merchant order ID |
| `SubmitTipAsync()` | Submit a tip for a completed delivery |

## Configuration

### GrabExpressConfig Properties

```csharp
public class GrabExpressConfig
{
    public string ClientId { get; set; }           // Your Grab API client ID
    public string ClientSecret { get; set; }       // Your Grab API client secret
    public string Environment { get; set; }        // "staging" or "production"
    public string? WebhookUrl { get; set; }        // Optional webhook URL
}
```

### Service Types

- `INSTANT` - Immediate delivery
- `SAME_DAY` - Same day delivery
- `SCHEDULED` - Scheduled delivery (requires Schedule parameter)

### Vehicle Types

- `MOTORCYCLE` - Motorcycle delivery
- `CAR` - Car delivery
- `VAN` - Van delivery

## Error Handling

The SDK throws `GrabExpressException` for API errors:

```csharp
try
{
    var delivery = await client.CreateDeliveryAsync(request);
}
catch (GrabExpressException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Error Details: {ex.ErrorMessage}");
}
```

## Rate Limits

**Production:**
- Quotes: 300 requests/second
- Other endpoints: 33 requests/second
- Tips: 5 requests/second

**Staging:**
- All endpoints: 5 requests/second

## Dependencies

- .NET 6.0 or higher
- System.Net.Http
- System.Text.Json

## Thread Safety

The `GrabExpressClient` is thread-safe and can be used concurrently. Token management is handled automatically with proper locking mechanisms.

## Best Practices

1. **Reuse Client Instance**: Create one client instance and reuse it throughout your application
2. **Dispose Properly**: Use `using` statements or call `Dispose()` when done
3. **Handle Exceptions**: Always wrap API calls in try-catch blocks
4. **Validate Input**: Ensure coordinates and addresses are valid before making requests
5. **Test in Staging**: Always test in staging environment before production

## Examples

See the `Examples/GrabExpressExamples.cs` file for complete working examples of all SDK features.

## Support

For API documentation, visit: https://developer.grab.com/docs/grab-express

## License

This SDK is provided as-is for integration with Grab Express API.
