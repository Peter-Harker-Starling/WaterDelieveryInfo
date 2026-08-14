namespace water.Models
{
    public class WaterDeliveryViewModel
    {
        public WaterDeliveryInfo Record { get; set; } = null!;
        public int? DaysSincePrevious { get; set; }
    }
}
