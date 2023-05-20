using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Cvijecara_Sanja_Tica_IT80_2019.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        public PaymentService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Korpa korpa)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            var service = new PaymentIntentService();
            var intent = new PaymentIntent();
            var subtotal = korpa.UkupanIznos;
            var deliveryFree = subtotal > 50 ? 0 : 3;
            if(string.IsNullOrEmpty(korpa.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long?)(subtotal + deliveryFree),
                    Currency = "EUR",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long?)subtotal + deliveryFree
                };
                await service.UpdateAsync(korpa.PaymentIntentId, options);
            }
            return intent;
        }
    }
}
