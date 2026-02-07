using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using GrabExpressApi.SDK.Configuration;
using GrabExpressApi.SDK.Models;

namespace GrabExpressApi.SDK;

/// <summary>
/// Main SDK client for Grab Express API
/// Handles authentication and provides methods for all API endpoints
/// </summary>
public class GrabExpressClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly GrabExpressConfig _config;
    private string? _accessToken;
    private DateTime _tokenExpiry;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    public GrabExpressClient(GrabExpressConfig config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_config.BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public GrabExpressClient(GrabExpressConfig config, HttpClient httpClient)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #region Authentication

    /// <summary>
    /// Authenticates and retrieves an access token using OAuth 2.0 client credentials flow
    /// </summary>
    private async Task<string> GetAccessTokenAsync()
    {
        await _tokenLock.WaitAsync();
        try
        {
            // Return cached token if still valid
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return _accessToken;
            }

            using var tokenClient = new HttpClient();
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _config.ClientId),
                new KeyValuePair<string, string>("client_secret", _config.ClientSecret),
                new KeyValuePair<string, string>("scope", _config.Scope)
            });

            var response = await tokenClient.PostAsync(_config.TokenUrl, requestContent);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new GrabExpressException(
                    "Failed to authenticate with Grab API",
                    (int)response.StatusCode,
                    errorMessage: content
                );
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.Access_Token))
            {
                throw new GrabExpressException("Invalid token response from Grab API");
            }

            _accessToken = tokenResponse.Access_Token;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.Expires_In - 60); // Refresh 60 seconds early

            return _accessToken;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    /// <summary>
    /// Ensures the HTTP client has a valid authorization header
    /// </summary>
    private async Task EnsureAuthenticatedAsync()
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    #endregion

    #region Delivery Quotes

    /// <summary>
    /// Get delivery quotes before placing an order
    /// </summary>
    /// <param name="request">Quote request details</param>
    /// <returns>Available quotes with pricing</returns>
    public async Task<DeliveryQuoteResponse> GetDeliveryQuotesAsync(DeliveryQuoteRequest request)
    {
        await EnsureAuthenticatedAsync();

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/v1/deliveries/quotes", content);

        return await HandleResponseAsync<DeliveryQuoteResponse>(response);
    }

    #endregion

    #region Create Delivery

    /// <summary>
    /// Create a new delivery request
    /// </summary>
    /// <param name="request">Delivery request details</param>
    /// <returns>Created delivery information</returns>
    public async Task<DeliveryResponse> CreateDeliveryAsync(CreateDeliveryRequest request)
    {
        await EnsureAuthenticatedAsync();

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/v1/deliveries", content);

        return await HandleResponseAsync<DeliveryResponse>(response);
    }

    #endregion

    #region Get Delivery Details

    /// <summary>
    /// Get details of a specific delivery
    /// </summary>
    /// <param name="deliveryId">The delivery ID</param>
    /// <returns>Delivery details</returns>
    public async Task<DeliveryResponse> GetDeliveryDetailsAsync(string deliveryId)
    {
        if (string.IsNullOrWhiteSpace(deliveryId))
            throw new ArgumentException("Delivery ID cannot be empty", nameof(deliveryId));

        await EnsureAuthenticatedAsync();

        var response = await _httpClient.GetAsync($"/v1/deliveries/{deliveryId}");
        return await HandleResponseAsync<DeliveryResponse>(response);
    }

    #endregion

    #region Cancel Delivery

    /// <summary>
    /// Cancel a specific delivery by delivery ID
    /// </summary>
    /// <param name="deliveryId">The delivery ID to cancel</param>
    public async Task CancelDeliveryAsync(string deliveryId)
    {
        if (string.IsNullOrWhiteSpace(deliveryId))
            throw new ArgumentException("Delivery ID cannot be empty", nameof(deliveryId));

        await EnsureAuthenticatedAsync();

        var response = await _httpClient.DeleteAsync($"/v1/deliveries/{deliveryId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new GrabExpressException(
                $"Failed to cancel delivery {deliveryId}",
                (int)response.StatusCode,
                errorMessage: errorContent
            );
        }
    }

    /// <summary>
    /// Cancel all deliveries associated with a merchant order ID
    /// </summary>
    /// <param name="merchantOrderId">The merchant order ID</param>
    public async Task CancelDeliveryByMerchantOrderIdAsync(string merchantOrderId)
    {
        if (string.IsNullOrWhiteSpace(merchantOrderId))
            throw new ArgumentException("Merchant Order ID cannot be empty", nameof(merchantOrderId));

        await EnsureAuthenticatedAsync();

        var response = await _httpClient.DeleteAsync($"/v1/merchant/deliveries/{merchantOrderId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new GrabExpressException(
                $"Failed to cancel delivery with merchant order ID {merchantOrderId}",
                (int)response.StatusCode,
                errorMessage: errorContent
            );
        }
    }

    #endregion

    #region Submit Tip

    /// <summary>
    /// Submit a tip for a completed delivery
    /// </summary>
    /// <param name="request">Tip request details</param>
    /// <returns>Tip submission response</returns>
    public async Task<SubmitTipResponse> SubmitTipAsync(SubmitTipRequest request)
    {
        await EnsureAuthenticatedAsync();

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/v1/deliveries/tip/submit", content);

        return await HandleResponseAsync<SubmitTipResponse>(response);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Handles HTTP response and deserializes to specified type
    /// </summary>
    private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new GrabExpressException(
                $"API request failed with status {response.StatusCode}",
                (int)response.StatusCode,
                errorMessage: content
            );
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (result == null)
            {
                throw new GrabExpressException("Failed to deserialize API response");
            }

            return result;
        }
        catch (JsonException ex)
        {
            throw new GrabExpressException($"Failed to parse API response: {ex.Message}");
        }
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        _httpClient?.Dispose();
        _tokenLock?.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}
