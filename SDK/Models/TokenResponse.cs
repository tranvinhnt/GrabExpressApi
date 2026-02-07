namespace GrabExpressApi.SDK.Models
{

    public class TokenResponse
    {
        public string Access_Token { get; set; } = string.Empty;
        public string Token_Type { get; set; } = string.Empty;
        public int Expires_In { get; set; }
    }

}