using ShipmentManagement.Application.DTOs.Shipment;

namespace ShipmentManagement.Application.Interfaces
{
    public interface IShipmentService
    {
        Task<ShipmentResponse> CreateShipmentAsync(
            CreateShipmentRequest request);
    }
}