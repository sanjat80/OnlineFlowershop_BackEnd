using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/placanje")]
    public class PaymentsController:ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly CvijecaraContext _context;
        private readonly IKorpaRepository _korpaRepository;
        private readonly IMapper _mapper;
        public PaymentsController(PaymentService paymentService, CvijecaraContext context, IKorpaRepository korpaRepository, IMapper mapper)
        {
            _context = context;
            _paymentService = paymentService;
            _korpaRepository = korpaRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "admin, registrovani")]
        public async Task<ActionResult<KorpaDto>> CreateOrUpdatePaymentIntent()
        {
            var korpa = _korpaRepository.GetKorpaFromLoggedUser();
            if (korpa == null) return NotFound("Nije pronadjena korpa dtog korisnika");

            var intent = await _paymentService.CreateOrUpdatePaymentIntent(korpa);
            if (intent == null) return BadRequest(new ProblemDetails { Title = "Problem prilikom kreiranja payment intent-a" });
            korpa.PaymentIntentId = korpa.PaymentIntentId ?? intent.Id.ToString();
            korpa.ClientSecret = korpa.ClientSecret ?? intent.ClientSecret;

            _context.Update(korpa);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest(new ProblemDetails { Title = "Problem izmjene payment intenta date korpe" });
            return _mapper.Map<KorpaDto>(korpa);
        }
    }
}
