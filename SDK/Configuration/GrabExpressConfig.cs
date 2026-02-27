namespace GrabExpressApi.SDK.Configuration
{

    public class GrabExpressConfig
    {
        public string ClientId { get; set; } = string.Empty;

        public string accessToken { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Environment { get; set; } = "staging"; // "staging" or "production"
        public string? WebhookUrl { get; set; }

        public string BaseUrl => Environment.ToLower() == "production"

          ? "https://partner-api.grab.com/grab-express-sandbox"
        : "https://partner-api.grab.com/grab-express-sandbox";

        public string TokenUrl => "https://partner-api.grab.com/grabid/v1/oauth2/token";

        public string Scope => "grab_express.partner_deliveries";
    }

}