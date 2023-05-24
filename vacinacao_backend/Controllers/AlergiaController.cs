using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vacinacao_backend.Models;
using vacinacao_backend.Services;

namespace vacinacao_backend.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AlergiaController : ControllerBase {

        private readonly AlergiaService _alergiaService;

        public AlergiaController(AlergiaService alergiaService) {
            _alergiaService = alergiaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Alergia>>> GetAllAlergias() {
            try {
                var alergias = await _alergiaService.FindAllAlergias();
                return Ok(alergias);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult> PostAlergia([FromBody] Alergia alergia) {
            try {
                await _alergiaService.InsertAlergia(alergia);
                return StatusCode(201);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlergia(int id) {
            try {
                await _alergiaService.DeleteAlergia(id);
                return NoContent();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
                throw;
            }
        }
    }
}
