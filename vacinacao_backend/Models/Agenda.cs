using vacinacao_backend.Models.Enums;

namespace vacinacao_backend.Models {
    public class Agenda {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public EnumSituacao Situacao { get; set; }
        public DateTime? DataSituacao { get; set; }
        public string? Observacoes { get; set; }

        public int VacinaId { get; set; }
        public Vacina? Vacina { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public Agenda() { }

        public Agenda(Agenda agenda) {
            Data = agenda.Data;
            Situacao = agenda.Situacao;
            DataSituacao = agenda.DataSituacao;
            Observacoes = agenda.Observacoes;
            VacinaId = agenda.VacinaId;
            UsuarioId = agenda.UsuarioId;
        }
    }
}
