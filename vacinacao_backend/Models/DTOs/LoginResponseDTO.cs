namespace vacinacao_backend.Models.DTOs {
    public struct LoginResponseDTO {
        public string AccessToken { get; set; }
        public int UsuarioId { get; set; }
    }
}
