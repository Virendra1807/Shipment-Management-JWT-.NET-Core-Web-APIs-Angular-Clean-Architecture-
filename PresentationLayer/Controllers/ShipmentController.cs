using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShipmentManagement.Application.DTOs.Shipment;
using ShipmentManagement.Application.Interfaces;

namespace PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment(CreateShipmentRequest request)
        {
            var result = await _shipmentService.CreateShipmentAsync(request);

            return Ok(result);
        }
    }
}
