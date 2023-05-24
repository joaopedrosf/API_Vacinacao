using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vacinacao_backend.Models;
using vacinacao_backend.Models.DTOs;
using vacinacao_backend.Services;

namespace vacinacao_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase {

        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService) {
            _usuarioService = usuarioService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAllUsuarios() {
            try {
                var usuarios = await _usuarioService.FindAllUsuarios();
                return Ok(usuarios);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(int id) {
            try {
                var usuario = await _usuarioService.FindUsuarioById(id);
                return Ok(usuario);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostUsuario([FromBody] InsertUsuarioDTO usuario) {
            try {
                await _usuarioService.InsertUsuario(usuario);
                return StatusCode(201);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("admin")]
        public async Task<ActionResult> PostUsuarioAdmin([FromBody] InsertUsuarioDTO usuario) {
            try {
                await _usuarioService.InsertUsuarioAdmin(usuario);
                return StatusCode(201);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsuario(int id) {
            try {
                await _usuarioService.DeleteUsuario(id);
                return NoContent();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
