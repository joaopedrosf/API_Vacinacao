using Microsoft.EntityFrameworkCore;
using vacinacao_backend.Exceptions;
using vacinacao_backend.Hubs;
using vacinacao_backend.Models;
using vacinacao_backend.Models.DTOs;
using vacinacao_backend.Models.Enums;
using vacinacao_backend.Repositories;

namespace vacinacao_backend.Services {
    public class AgendaService {

        private readonly VacinacaoContext _vacinacaoContext;
        private readonly VacinacaoHub _vacinacaoHub;

        public AgendaService(VacinacaoContext vacinacaoContext, VacinacaoHub vacinacaoHub) {
            _vacinacaoContext = vacinacaoContext;
            _vacinacaoHub = vacinacaoHub;
        }

        public async Task<List<Agenda>> FindAllAgendamentos() {
            return await _vacinacaoContext.Agendamentos.Include(a => a.Usuario).AsNoTracking().Include(a => a.Vacina).AsNoTracking().ToListAsync();
        }

        public async Task<List<Agenda>> FindAgendamentosBySituacao(EnumSituacao situacao) {
            return await _vacinacaoContext.Agendamentos.Where(a => a.Situacao == situacao).Include(a => a.Usuario).AsNoTracking().Include(a => a.Vacina).AsNoTracking().ToListAsync();
        }

        public async Task<List<Agenda>> FindAgendamentosByData(DateTime data) {
            return await _vacinacaoContext.Agendamentos.Where(a => a.Data.Date == data.ToUniversalTime().Date).Include(a => a.Usuario).AsNoTracking().Include(a => a.Vacina).AsNoTracking().ToListAsync();
        }

        public async Task InsertAgendamento(Agenda agendamento) {
            var usuario = await _vacinacaoContext.Usuarios.AsNoTracking().Include(u => u.Alergias).AsNoTracking().FirstOrDefaultAsync(a => a.Id == agendamento.UsuarioId);

            if(usuario == null) {
                throw new UsuarioNaoEncontradoException("Usuário não encontrado!");
            }
            if(!usuario.IsUsuarioMaiorDeIdade()) {
                throw new UsuarioMenorDeIdadeException("Usuário menor de idade não pode fazer agendamentos!");
            }

            var vacina = await _vacinacaoContext.Vacinas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == agendamento.VacinaId);

            if(vacina == null) {
                throw new VacinaNaoEncontradaException("Vacina não encontrada!");
            }

            if (usuario.Alergias != null && usuario.Alergias.Any(a => a.VacinaId != null && a.VacinaId == vacina.Id)) {
                throw new AgendamentoInvalidoException("Não é possível fazer o agendamento para uma vacina a qual o usuário tenha alergia");
            }

            var agendamentosDaVacinaParaUsuario = _vacinacaoContext.Agendamentos.Where(a => a.VacinaId == agendamento.VacinaId && a.UsuarioId == agendamento.UsuarioId).ToList();

            if(agendamentosDaVacinaParaUsuario.Any(a => a.Situacao == EnumSituacao.Agendado)) {
                throw new AgendamentoInvalidoException("Não é possível agendar uma vacina para a qual o usuário já possui agendamentos");
            }

            int dosesRealizadas = agendamentosDaVacinaParaUsuario.Count(a => a.Situacao == EnumSituacao.Realizado);
            if (vacina.Doses == dosesRealizadas) {
                throw new AgendamentoInvalidoException("Não é possível agendar uma vacina para a qual o usuário já tomou todas as doses");
            }

            int dosesRestantes = vacina.Doses - dosesRealizadas;

            var listaAgendamentosRestantes = new List<Agenda>(dosesRestantes);
            await _vacinacaoContext.Agendamentos.AddAsync(agendamento);
            listaAgendamentosRestantes.Add(agendamento);
            for (int i = 1; i < dosesRestantes; i++) {
                var proximaData = agendamento.Data;
                int novoIntervalo = vacina.Intervalo!.Value * i;
                switch (vacina.Periodicidade) {
                    case EnumPeriodicidade.Dias:
                        proximaData = proximaData.AddDays(novoIntervalo);
                        break;
                    case EnumPeriodicidade.Semanas:
                        proximaData = proximaData.AddDays(novoIntervalo * 7);
                        break;
                    case EnumPeriodicidade.Meses:
                        proximaData = proximaData.AddMonths(novoIntervalo);
                        break;
                    case EnumPeriodicidade.Anos:
                        proximaData = proximaData.AddYears(novoIntervalo);
                        break;
                }
                var novaAgenda = new Agenda(agendamento);
                novaAgenda.Data = proximaData;
                await _vacinacaoContext.Agendamentos.AddAsync(novaAgenda);
                listaAgendamentosRestantes.Add(novaAgenda);
            }
            await _vacinacaoContext.SaveChangesAsync();
            await _vacinacaoHub.SendNovosAgendamentos(listaAgendamentosRestantes.ConvertAll(a => new AgendaDTO(a, usuario, vacina)));
            _vacinacaoContext.Agendamentos.Entry(agendamento).State = EntityState.Detached;
        }

        public async Task DeleteAgendamento(int id) {
            var agendamento = _vacinacaoContext.Agendamentos.Where(a => a.Id == id).FirstOrDefault();
            if(agendamento.Situacao != EnumSituacao.Agendado) {
                throw new InvalidOperationException("Não é possível deletar um agendamento que já foi concluído");
            }
            var agendamentosVacinaUsuario = await _vacinacaoContext.Agendamentos.Where(a => a.VacinaId == agendamento.VacinaId && a.UsuarioId == agendamento.UsuarioId && a.Situacao == EnumSituacao.Agendado).ToListAsync();
            _vacinacaoContext.Agendamentos.RemoveRange(agendamentosVacinaUsuario);
            await _vacinacaoContext.SaveChangesAsync();
        }

        public async Task UpdateSituacaoAgendamento(int id, EnumSituacao situacao, string observacoes) {
            var agendamento = await _vacinacaoContext.Agendamentos.Where(a => a.Id == id).FirstAsync();
            _vacinacaoContext.Agendamentos.Attach(agendamento);
            agendamento.Situacao = situacao;
            agendamento.Observacoes = observacoes;
            agendamento.DataSituacao = DateTime.UtcNow;
            await _vacinacaoContext.SaveChangesAsync();
        }

        public async Task<List<Agenda>> FindAgendamentosByUsuario(int usuarioId) {
            return await _vacinacaoContext.Agendamentos.Where(a => a.UsuarioId == usuarioId).Include(a => a.Vacina).AsNoTracking().ToListAsync();
        }
    }
}
