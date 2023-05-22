using Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IdentityModel.Tokens.Jwt;

namespace Cvijecara_Sanja_Tica_IT80_2019.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IKorpaRepository _korpaRepository;
        public PaymentService(IConfiguration config, IKorpaRepository korpaRepository)
        {
            _config = config;
            _korpaRepository = korpaRepository;
        }

        public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Korpa korpa)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            var service = new PaymentIntentService();
            var intent = new PaymentIntent();
            var subtotal = _korpaRepository.UpdateKorpaDetails(korpa.KorpaId);
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
