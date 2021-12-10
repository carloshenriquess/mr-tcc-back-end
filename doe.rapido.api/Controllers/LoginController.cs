using doe.rapido.api.Models;
using doe.rapido.api.Services;
using doe.rapido.business.BLL;
using doe.rapido.business.DML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using static doe.rapido.api.Services.TokenService;

namespace doe.rapido.api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        #region[construtores]
        BusinessUser businessUser = new BusinessUser();
        BusinessCompany businessCompany = new BusinessCompany();
        private readonly IMailService _mailService;
        #endregion
        #region[atributos]
        public LoginController(IMailService mailService)
        {
            _mailService = mailService;
        }
        #endregion
        #region[métodos post]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> AuthenticateAsync([FromBody] LoginModel model)
        {
            User? user = await businessUser.GetUserByEmail(model.email);
            if (user == null)
                return NotFound("Usuário não encontrado");
            HashService hashService = new HashService();
            if (hashService.CreateHash(model.password) != user.Password)
                return NotFound("Email ou senha inválidos");
            if (user.Confirmed == false)
                return Unauthorized("Email ainda não confirmado");
            var login = Login(user);
            user.Password = "";
            Company? company = await businessCompany.GetCompanyByIdUser(user.Id);
            return new
            {
                user = user,
                company = company,
                access = login.Value
            };
        }
        [HttpPost]
        [Route("confirm-email")]
        public async Task<ActionResult<dynamic>> ConfirmEmail([FromBody] LoginModel.ConfirmEmail confirmemail)
        {
            if (String.IsNullOrEmpty(confirmemail.email) || confirmemail.code == null)
            {
                return BadRequest("Email ou código não podem ser nulos");
            }
            bool? confirmedUser = await businessUser.ConfirmUser(confirmemail.email, (int)confirmemail.code);
            if (confirmedUser == true)
            {
                User? user = await businessUser.GetUserByEmail(confirmemail.email);
                var login = Login(user);
                user.Password = "";
                Company? company = await businessCompany.GetCompanyByIdUser(user.Id);
                return new
                {
                    user = user,
                    company = company,
                    access = login.Value
                };
            }
            return BadRequest("Código inválido");
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult<string>> ChangePassword([FromBody] LoginModel.ChangePassword changePassword)
        {
            if (String.IsNullOrEmpty(changePassword.password) || (changePassword.code == null))
            {
                return BadRequest("Senha ou código não podem ser nulos");
            }
            HashService hashService = new HashService();
            User user = await businessUser.GetUserByEmail(changePassword.email);
            if (user.CodeConfirm != changePassword.code || DateTime.Now > user.DtExpireCodeConfirm)
                return BadRequest("O código informado é inválido");

            user.Password = hashService.CreateHash(changePassword.password);
            user.CodeConfirm = null;
            user.DtUpdate = DateTime.Now;
            await businessUser.UpdateUser(user);
            await TokenService.DeleteRefreshToken(user.Id);
            return Ok("Senha alterada com sucesso");
        }
        [HttpPost]
        [Authorize]
        [Route("email-change-login")]
        public async Task<ActionResult<string>> EmailToChangeLogin()
        {
            DateTime data = DateTime.Now;
            data = data.AddMinutes(10);
            int code = businessUser.RandomNumber();
            int currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")!.Value);
            User user = await businessUser.GetUserById(currentUserId);
            user.CodeConfirm = code;
            user.DtExpireCodeConfirm = data;
            await businessUser.UpdateUser(user);
            SendMailViewModel email = new SendMailViewModel();
            email.Emails = new String[] { user.Email };
            email.Subject = "Doe Rápido - Redefinição de Login";
            email.Body = "Seu código para redefinir login é: " + code;
            _mailService.sendEmail(email.Emails, email.Subject, email.Body, false);
            return Ok();
        }
        [HttpPost]
        [Route("email-change-password")]
        public async Task<ActionResult<string>> EmailToChangePassword(LoginModel.EmailToChangePassword emailToChangePassword)
        {
            DateTime data = DateTime.Now;
            data = data.AddMinutes(10);
            int code = businessUser.RandomNumber();
            User user = await businessUser.GetUserByEmail(emailToChangePassword.email);
            user.CodeConfirm = code;
            user.DtExpireCodeConfirm = data;
            await businessUser.UpdateUser(user);
            SendMailViewModel sendEmail = new SendMailViewModel();
            sendEmail.Emails = new String[] { user.Email };
            sendEmail.Subject = "Doe Rápido - Redefinição de Senha";
            sendEmail.Body = "Seu código para redefinir senha é: " + code;
            _mailService.sendEmail(sendEmail.Emails, sendEmail.Subject, sendEmail.Body, false);
            return Ok("Código enviado para o email: " + user.Email);
        }
        [HttpPost]
        [Authorize]
        [Route("change-login")]
        public async Task<ActionResult<string>> ChangeLogin([FromBody] LoginModel.ChangeLogin login)
        {
            if (String.IsNullOrEmpty(login.email))
            {
                return BadRequest("Email não pode ser nulo");
            }
            int currentUserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")!.Value);
            User user = await businessUser.GetUserById(currentUserId);
            if (user.CodeConfirm != login.code || DateTime.Now > user.DtExpireCodeConfirm)
                return BadRequest("O código informado é inválido");
            user.Email = login.email;
            user.CodeConfirm = null;
            await businessUser.UpdateUser(user);
            await TokenService.DeleteRefreshToken(user.Id);
            return Ok("Login alterado com sucesso");
        }
        [HttpPost]
        [Route("logoff/{userId}")]
        public async Task<ActionResult<string>> ChangeLogin(int userId)
        {
            await TokenService.DeleteRefreshToken(userId);
            return Ok();
        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult> RefreshToken(RefreshToken _refreshToken)
        {
            string token = _refreshToken.token;
            string refreshToken = _refreshToken.refreshToken;
            var principal = TokenService.GetPrincipalFromExpiredToken(token);
            int currentUserId = int.Parse(principal.Claims.FirstOrDefault(c => c.Type == "id")!.Value);
            var savedRefreshToken = TokenService.GetRefreshToken(currentUserId);
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Token inválido");
            var newJwtToken = TokenService.GenerateToken(principal.Claims);
            var newRefreshToken = TokenService.GenerateRefreshToken();
            await TokenService.DeleteRefreshToken(currentUserId);
            TokenService.SaveRefreshToken(currentUserId, newRefreshToken);
            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }
        #endregion
        #region[métodos privados]
        private ActionResult<dynamic> Login(User user)
        {
            string token = TokenService.GenerateToken(user);
            string refreshToken = TokenService.GenerateRefreshToken();
            TokenService.SaveRefreshToken(user.Id, refreshToken);
            return new
            {
                token = token,
                refreshToken = refreshToken
            };
        }
        #endregion
    }
}
