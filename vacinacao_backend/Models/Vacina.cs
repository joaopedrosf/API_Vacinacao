using vacinacao_backend.Models.Enums;

namespace vacinacao_backend.Models {
    public class Vacina {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Doses { get; set; }
        public EnumPeriodicidade? Periodicidade { get; set; }
        public int? Intervalo { get; set; }

        public List<Agenda>? Agendamentos { get; set; }
    }
}
