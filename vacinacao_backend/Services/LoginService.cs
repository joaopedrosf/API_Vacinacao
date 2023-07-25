using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using vacinacao_backend.Models;
using vacinacao_backend.Models.DTOs;
using vacinacao_backend.Repositories;

namespace vacinacao_backend.Services {
    public class LoginService {

        private readonly VacinacaoContext _vacinacaoContext;
        private readonly TokenService _tokenService;

        public LoginService(VacinacaoContext vacinacaoContext, TokenService tokenService) {
            _vacinacaoContext = vacinacaoContext;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request) {
			try {
                var usuario = await _vacinacaoContext.Usuarios.Where(u => u.Email == request.Email).SingleAsync();
                if(!BCrypt.Net.BCrypt.EnhancedVerify(request.Senha, usuario.Senha)) {
                    throw new AuthenticationException();
                }
                var response = new LoginResponseDTO { AccessToken = _tokenService.GenerateAccessToken(usuario), UsuarioId = usuario.Id };
                return response;
			}
			catch (Exception) {
                throw new AuthenticationException("Email ou senha inválidos!");
			}
        }
    }
}
