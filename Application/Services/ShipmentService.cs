using ShipmentManagement.Application.DTOs.Shipment;
using ShipmentManagement.Application.Interfaces;
using ShipmentManagement.Domain.Entities;

namespace ShipmentManagement.Application.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly IGenericRepository<Shipment> _repository;

        public ShipmentService(IGenericRepository<Shipment> repository)
        {
            _repository = repository;
        }

        public async Task<ShipmentResponse> CreateShipmentAsync(
            CreateShipmentRequest request)
        {
            var shipment = new Shipment
            {
                SenderName = request.SenderName,
                ReceiverName = request.ReceiverName,
                Origin = request.Origin,
                Destination = request.Destination,
                TrackingNumber = GenerateTrackingNumber(),
                Status = "Created",
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(shipment);

            await _repository.SaveChangesAsync();

            return new ShipmentResponse
            {
                ShipmentId = shipment.ShipmentId,
                TrackingNumber = shipment.TrackingNumber,
                SenderName = shipment.SenderName,
                ReceiverName = shipment.ReceiverName,
                Origin = shipment.Origin,
                Destination = shipment.Destination,
                Status = shipment.Status,
                CreatedDate = shipment.CreatedDate
            };
        }

        private string GenerateTrackingNumber()
        {
            return $"SHP-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        }
    }
}