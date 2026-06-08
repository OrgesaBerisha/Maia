using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaymentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("create-intent")]
        public async Task<IActionResult> CreateIntent([FromBody] CreateIntentDto dto)
        {
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

            var options = new PaymentIntentCreateOptions
            {
                Amount             = (long)(dto.Amount * 100),
                Currency           = "eur",
                PaymentMethodTypes = new List<string> { "card" },
                Description        = "MAIA Order Payment",
            };

            var service = new PaymentIntentService();
            var intent  = await service.CreateAsync(options);

            return Ok(new
            {
                clientSecret    = intent.ClientSecret,
                publishableKey  = _config["Stripe:PublishableKey"]
            });
        }
    }

    public class CreateIntentDto
    {
        public decimal Amount { get; set; }
    }
}
