using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context, ApplicationUser user);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}