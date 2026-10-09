namespace ShipmentManagement.Application.DTOs.Shipment
{
    public class ShipmentResponse
    {
        public int ShipmentId { get; set; }

        public string TrackingNumber { get; set; } = string.Empty;

        public string SenderName { get; set; } = string.Empty;

        public string ReceiverName { get; set; } = string.Empty;

        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}