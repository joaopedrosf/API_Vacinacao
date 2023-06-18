using vacinacao_backend.Models.Enums;

namespace vacinacao_backend.Models.DTOs {
    public class VacinaDTO {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Doses { get; set; }
        public EnumPeriodicidade? Periodicidade { get; set; }
        public int? Intervalo { get; set; }

        public VacinaDTO(Vacina vacina) {
            Id = vacina.Id;
            Titulo = vacina.Titulo;
            Descricao = vacina.Descricao;
            Doses = vacina.Doses;
            Periodicidade = vacina.Periodicidade;
            Intervalo = vacina.Intervalo;
        }
    }
}
