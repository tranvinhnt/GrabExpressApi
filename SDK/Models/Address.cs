namespace GrabExpressApi.SDK.Models
{

    public class Address
    {
        public string AddressLine { get; set; } = string.Empty;
        public Coordinates Coordinates { get; set; } = new Coordinates();
        public string? CityCode { get; set; }
        public string? Keywords { get; set; }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}