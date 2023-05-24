using vacinacao_backend.Models.DTOs;

namespace vacinacao_testes {
    public class UsuarioTest {

        [Fact]
        public async Task InsertUsuarioTest() {
            var usuarioService = ServiceFactory.CreateUsuarioService();
            
            var usuario = new InsertUsuarioDTO() { Nome = "Nome Teste", DataNascimento = new DateOnly(1996, 08, 23), Sexo = 'M', Logradouro = "Rua 2", Numero = 150, Setor = "Centro", Cidade = "Goiânia", UF = "GO", Email = "emailteste@teste.com", Senha = "123" };
            await usuarioService.InsertUsuario(usuario);

            var usuarios = await usuarioService.FindAllUsuarios();

            Assert.Single(usuarios);
        }

        [Fact]
        public async Task DeleteUsuarioByIdTest() {
            var usuarioService = ServiceFactory.CreateUsuarioService();

            var usuario = new InsertUsuarioDTO() { Nome = "Nome completo", DataNascimento = new DateOnly(1989, 05, 14), Sexo = 'M', Logradouro = "Rua 2", Numero = 150, Setor = "Centro", Cidade = "Goiânia", UF = "GO", Email = "emailteste@teste.com", Senha = "123" };
            await usuarioService.InsertUsuario(usuario);
            var usuarios = await usuarioService.FindAllUsuarios();

            await usuarioService.DeleteUsuario(usuarios[0].Id);

            usuarios = await usuarioService.FindAllUsuarios();
            Assert.Empty(usuarios);
        }

        [Fact]
        public async Task FindUsuarioByIdTest() {
            var usuarioService = ServiceFactory.CreateUsuarioService();

            var usuarioDTO = new InsertUsuarioDTO() { Nome = "Unique User", DataNascimento = new DateOnly(2001, 03, 19), Sexo = 'M', Logradouro = "Rua 2", Numero = 150, Setor = "Centro", Cidade = "Goiânia", UF = "GO", Email = "emailteste@teste.com", Senha = "123" };
            await usuarioService.InsertUsuario(usuarioDTO);

            var usuario = await usuarioService.FindUsuarioById(1);

            Assert.NotNull(usuario);
            Assert.Equal("Unique User", usuario.Nome);
        }

        [Fact]
        public async Task FindUsuarioByIdInexistenteDeveriaRetornarException() {
            var usuarioService = ServiceFactory.CreateUsuarioService();

            await Assert.ThrowsAsync<ArgumentException>(async () => { await usuarioService.FindUsuarioById(50); });
        }
    }
}