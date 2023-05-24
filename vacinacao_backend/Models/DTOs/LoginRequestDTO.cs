namespace vacinacao_backend.Models.DTOs {
    public record LoginRequestDTO {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
