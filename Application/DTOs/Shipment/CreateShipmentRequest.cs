namespace ShipmentManagement.Application.DTOs.Shipment
{
    public class CreateShipmentRequest
    {
        public string SenderName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
    }
}