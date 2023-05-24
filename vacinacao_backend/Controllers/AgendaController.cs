using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using vacinacao_backend.Models;
using vacinacao_backend.Models.Enums;
using vacinacao_backend.Services;

namespace vacinacao_backend.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase {

        private readonly AgendaService _agendaService;

        public AgendaController(AgendaService agendaService) {
            _agendaService = agendaService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<ActionResult<List<Agenda>>> GetAllAgendamentos() {
            try {
                var agendamentos = await _agendaService.FindAllAgendamentos();
                return Ok(agendamentos);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("situacao")]
        public async Task<ActionResult<List<Agenda>>> GetAgendamentosBySituacao(
            [FromQuery(Name = "situacao")][Required(ErrorMessage = "O queryparam situacao é obrigatório")] EnumSituacao situacao) {
            try {
                var agendamentos = await _agendaService.FindAgendamentosBySituacao(situacao);
                return Ok(agendamentos);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("data")]
        public async Task<ActionResult<List<Agenda>>> GetAgendamentosByData(
            [FromQuery(Name = "data")][Required(ErrorMessage = "O queryparam data é obrigatório")] DateTime data) {
            try {
                var agendamentos = await _agendaService.FindAgendamentosByData(data);
                return Ok(agendamentos);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PostAgendamento([FromBody] Agenda agendamento) {
            try {
                await _agendaService.InsertAgendamento(agendamento);
                return StatusCode(201);
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAgendamento(int id) {
            try {
                await _agendaService.DeleteAgendamento(id);
                return NoContent();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("situacao")]
        public async Task<ActionResult> PutSituacaoAgendamento(
            [FromQuery(Name = "id")][Required(ErrorMessage = "O queryparam id é obrigatório")] int id,
            [FromQuery(Name = "situacao")][Required(ErrorMessage = "O queryparam situacao é obrigatório")] EnumSituacao situacao,
            [FromQuery(Name = "observacoes")][Required(ErrorMessage = "O queryparam observacoes é obrigatório")] string observacoes) {
            try {
                await _agendaService.UpdateSituacaoAgendamento(id, situacao, observacoes);
                return Ok();
            }
            catch (Exception e) {
                return StatusCode(500, e.Message);
            }
        }
    }
}
