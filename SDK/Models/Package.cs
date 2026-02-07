namespace GrabExpressApi.SDK.Models;

public class Package
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Dimensions? Dimensions { get; set; }
}

public class Dimensions
{
    public double Height { get; set; }
    public double Width { get; set; }
    public double Depth { get; set; }
    public double Weight { get; set; }
}
