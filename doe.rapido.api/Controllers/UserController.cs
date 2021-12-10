using doe.rapido.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text.Json;
using doe.rapido.business;
using doe.rapido.business.DML;
using doe.rapido.business.BLL;
using doe.rapido.api.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace doe.rapido.api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        #region[construtores]
        private readonly ILogger<UserController> _logger;
        BusinessUser businessUser = new BusinessUser();
        BusinessCompany businessCompany = new BusinessCompany();
        private readonly IMailService _mailService;
        #endregion
        #region[atributos]
        public UserController(ILogger<UserController> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }
        #endregion
        #region[MetodosPost]
        [HttpPost]
        [Route("user")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User? existUser = await businessUser.GetUserByEmail(user.Email);
            if (existUser != null && existUser.Confirmed == true)
                return BadRequest("Email informado já está cadastrado");

            HashService hashService = new HashService();
            DateTime data = DateTime.Now;
            data = data.AddMinutes(10);
            int code = businessUser.RandomNumber();
            user.DtExpireCodeConfirm = data;
            user.CodeConfirm = code;
            user.Password = hashService.CreateHash(user.Password);
            if (existUser != null && existUser.Confirmed == false)
            {
                await businessUser.UpdateUser(businessUser.MappingUser(existUser, user));
            }
            else
            {
                await businessUser.InsertUser(user);
            }
            SendMailViewModel email = new SendMailViewModel();
            email.Emails = new String[] { user.Email };
            email.Subject = "Doe Rápido - Confirmação de E-mail";
            email.Body = "Seu código para confirmar e-mail é: " + code;
            _mailService.sendEmail(email.Emails, email.Subject, email.Body, false);
            //gerar codigo +expiracao atualizar usuario e enviar email
            User createdUser = await businessUser.GetUserByEmail(user.Email);

            return Ok(createdUser);
        }
        [HttpPost]
        [Authorize]
        [Route("deleteUser/{userId}")]
        public async Task<ActionResult<string>> DeleteUser(int? userId)
        {
            if (userId == null)
            {
                return BadRequest("userId cannot be null");
            }
            User? user = await businessUser.GetUserById((int)userId);
            if (user != null)
                await businessUser.DeleteUserAndCompany((int)userId);
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("uploadImageBase64")]
        public async Task<ActionResult<string>> uploadImageBase64([FromBody] UserModel.Base64 valueBase64)
        {
            if (String.IsNullOrEmpty(valueBase64.base64image))
            {
                return BadRequest("String base64 não pode ser nula");
            }
            ImageService imageService = new ImageService();
            string url = await imageService.UploadBase64Image(valueBase64.base64image!);
            return Ok(url);
        }
        [HttpPost]
        [Route("send-new-code")]
        public async Task<ActionResult<string>> newCode([FromBody] LoginModel.ConfirmEmail confirmEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User? user = await businessUser.GetUserByEmail(confirmEmail.email);
            DateTime data = DateTime.Now;
            data = data.AddMinutes(10);
            int code = businessUser.RandomNumber();
            user.DtExpireCodeConfirm = data;
            user.CodeConfirm = code;
            await businessUser.UpdateUser(user);
            SendMailViewModel email = new SendMailViewModel();
            email.Emails = new String[] { user.Email };
            email.Subject = "Doe Rápido - Confirmação de E-mail";
            email.Body = "Seu novo código para confirmar e-mail é: " + code;
            _mailService.sendEmail(email.Emails, email.Subject, email.Body, false);
            return Ok();
        }
        #endregion
        #region [MetodosPut]
        [HttpPut]
        [Authorize]
        [Route("user/{userId}")]
        public async Task<ActionResult<string>> UpdateUser(int userId, [FromBody] User _user)
        {
            User? user = await businessUser.GetUserById(userId);
            if (user == null) return BadRequest("Usuário não encontrado");
            await businessUser.UpdateUser(businessUser.MappingUser(user, _user));
            return Ok(await businessUser.GetUserById(userId));
        }
        #endregion
        #region[MetodosGet]
        [HttpGet]
        [Authorize]
        [Route("user/{userId}")]
        public async Task<ActionResult<string>> GetUser(int? userId)
        {
            if (userId == null)
            {
                return BadRequest("userId não pode ser nulo");
            }
            User user = await businessUser.GetUserById((int)userId);
            return Ok(user);
        }
        #endregion
    }
}
