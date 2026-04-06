using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GrabExpressApi.SDK;
using GrabExpressApi.SDK.Models;

namespace GrabExpressApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly GrabExpressProvider _provider;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(GrabExpressProvider provider, ILogger<DeliveryController> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        private GrabExpressClient GrabClient 
        {
            get
            {
                var env = Request.Headers["X-Grab-Env"].ToString();
                return _provider.GetClient(env);
            }
        }

        /// <summary>
        /// Get delivery quotes
        /// </summary>
        [HttpPost("quotes")]
        public async Task<ActionResult<DeliveryQuoteResponse>> GetQuotes([FromBody] DeliveryQuoteRequest request)
        {
            try
            {
                var quotes = await GrabClient.GetDeliveryQuotesAsync(request);
                return Ok(quotes);
            }
            catch (GrabExpressException ex)
            {
                _logger.LogError(ex, "Error getting delivery quotes");
                return StatusCode(ex.StatusCode, new { error = ex.Message, details = ex.ErrorMessage });
            }
        }

        /// <summary>
        /// Create a new delivery
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<DeliveryResponse>> CreateDelivery([FromBody] CreateDeliveryRequest request)
        {
            try
            {
                var delivery = await GrabClient.CreateDeliveryAsync(request);
                return CreatedAtAction(nameof(GetDelivery), new { id = delivery.DeliveryID }, delivery);
            }
            catch (GrabExpressException ex)
            {
                _logger.LogError(ex, "Error creating delivery");
                return StatusCode(ex.StatusCode, new { error = ex.Message, details = ex.ErrorMessage });
            }
        }

        /// <summary>
        /// Get delivery details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryResponse>> GetDelivery(string id)
        {
            try
            {
                var delivery = await GrabClient.GetDeliveryDetailsAsync(id);
                return Ok(delivery);
            }
            catch (GrabExpressException ex)
            {
                _logger.LogError(ex, "Error getting delivery details for {DeliveryId}", id);
                return StatusCode(ex.StatusCode, new { error = ex.Message, details = ex.ErrorMessage });
            }
        }

        /// <summary>
        /// Cancel a delivery by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelDelivery(string id)
        {
            try
            {
                await GrabClient.CancelDeliveryAsync(id);
                return NoContent();
            }
            catch (GrabExpressException ex)
            {
                _logger.LogError(ex, "Error cancelling delivery {DeliveryId}", id);
                return StatusCode(ex.StatusCode, new { error = ex.Message, details = ex.ErrorMessage });
            }
        }

        /// <summary>
        /// Cancel delivery by merchant order ID
        /// </summary>
        [HttpDelete("merchant/{merchantOrderId}")]
        public async Task<ActionResult> CancelByMerchantOrderId(string merchantOrderId)
        {
            try
            {
                await GrabClient.CancelDeliveryByMerchantOrderIdAsync(merchantOrderId);
                return NoContent();
            }
            catch (GrabExpressException ex)
            {
                _logger.LogError(ex, "Error cancelling delivery by merchant order ID {MerchantOrderId}", merchantOrderId);
                return StatusCode(ex.StatusCode, new { error = ex.Message, details = ex.ErrorMessage });
            }
        }

        /// <summary>
        /// Submit a tip for a delivery
        /// </summary>
        [HttpPost("tip")]
        public async Task<ActionResult<SubmitTipResponse>> SubmitTip([FromBody] SubmitTipRequest request)
        {
            try
            {
                var response = await GrabClient.SubmitTipAsync(request);
                return Ok(response);
            }
            catch (GrabExpressException ex)
            {
                _logger.LogError(ex, "Error submitting tip for delivery {DeliveryId}", request.DeliveryID);
                return StatusCode(ex.StatusCode, new { error = ex.Message, details = ex.ErrorMessage });
            }
        }
    }

}