using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Stripe;

namespace Cvijecara_Sanja_Tica_IT80_2019.Services
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreateOrUpdatePaymentIntent(Korpa korpa);
    }
}
