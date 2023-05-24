namespace vacinacao_backend.Exceptions {
    public class UsuarioMenorDeIdadeException : Exception {
        public UsuarioMenorDeIdadeException(string? message) : base(message) { }
    }
}
