using Newtonsoft.Json;

namespace GrabExpressApi.SDK.Models
{

    public partial class Package
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }
    }
    public partial class Dimensions
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
    }

}