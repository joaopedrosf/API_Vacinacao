using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vacinacao_backend.Models;
using vacinacao_backend.Services;

namespace vacinacao_backend.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class VacinaController : ControllerBase {

        private readonly VacinaService _vacinaService;

        public VacinaController(VacinaService vacinaService) {
            _vacinaService = vacinaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vacina>>> GetAllVacinas() {
            try {
                var vacinas = await _vacinaService.FindAllVacinas();
                return Ok(vacinas);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<ActionResult> PostVacina([FromBody] Vacina vacina) {
            try {
                await _vacinaService.InsertVacina(vacina);
                return StatusCode(201);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVacina(int id) {
            try {
                await _vacinaService.DeleteVacina(id);
                return NoContent();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
