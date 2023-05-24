namespace vacinacao_backend.Models {
    public class Alergia {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? VacinaId { get; set; }
        public Vacina? Vacina { get; set; }
        public List<Usuario>? Usuarios { get; set; }
    }
}
