namespace GrabExpressApi.SDK.Models
{

    public class SubmitTipRequest
    {
        public string DeliveryID { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "SGD";
    }

    public class SubmitTipResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

}