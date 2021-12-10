using doe.rapido.api.Models;
using doe.rapido.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace doe.rapido.api.Controllers
{
    [ApiController]
    [Route("api/mail")]
    
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public IActionResult SendMail([FromBody] SendMailViewModel email)
        {
            _mailService.sendEmail(email.Emails, email.Subject, email.Body, email.IsHtml);
            return Ok();
        }
    }
}
