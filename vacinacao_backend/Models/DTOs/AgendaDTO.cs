using vacinacao_backend.Models.Enums;

namespace vacinacao_backend.Models.DTOs {
    public class AgendaDTO {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public EnumSituacao Situacao { get; set; }
        public DateTime? DataSituacao { get; set; }
        public string? Observacoes { get; set; }
        public VacinaDTO Vacina { get; set; }
        public UsuarioDTO Usuario { get; set; }

        public AgendaDTO(Agenda agenda, Usuario usuario, Vacina vacina) {
            Id = agenda.Id;
            Data = agenda.Data;
            Situacao = agenda.Situacao;
            DataSituacao = agenda.DataSituacao;
            Observacoes = agenda.Observacoes;
            Vacina = new VacinaDTO(vacina);
            Usuario = new UsuarioDTO(usuario);
        }
    }
}
