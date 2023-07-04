using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using vacinacao_backend.Models.DTOs;
using vacinacao_backend.Services;

namespace vacinacao_backend.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {

        private readonly LoginService _loginService;

        public LoginController(LoginService loginService) {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO request) {
            try {
                var response = await _loginService.Login(request);
                return Ok(response);
            }
            catch (AuthenticationException e) {
                return Unauthorized(e.Message);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("token/validate")]
        [Authorize]
        public ActionResult ValidateToken() {
            return Ok();
        }
    }
}
