namespace vacinacao_backend.Models.DTOs {
    public class UsuarioDTO {
        public int Id { get; set; }
        public string Nome { get; set; }

        public UsuarioDTO(Usuario usuario) {
            Id = usuario.Id;
            Nome = usuario.Nome;
        }
    }
}
