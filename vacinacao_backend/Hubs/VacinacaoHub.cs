using Microsoft.AspNetCore.SignalR;
using vacinacao_backend.Models;
using vacinacao_backend.Models.DTOs;

namespace vacinacao_backend.Hubs {
    public class VacinacaoHub : Hub {

        public async Task SendNovosAgendamentos(List<AgendaDTO> agendas) {
            if (Clients is not null) {
                await Clients.All.SendAsync("NovosAgendamentos", agendas);
            }
        }
    }
}
