using Microsoft.EntityFrameworkCore;
using vacinacao_backend.Exceptions;
using vacinacao_backend.Models;
using vacinacao_backend.Models.DTOs;
using vacinacao_backend.Repositories;

namespace vacinacao_backend.Services
{
    public class UsuarioService {

        private readonly VacinacaoContext _vacinacaoContext;

        public UsuarioService(VacinacaoContext vacinacaoContext) {
            _vacinacaoContext = vacinacaoContext;
        }

        public async Task<List<Usuario>> FindAllUsuarios() {
            return await _vacinacaoContext.Usuarios.AsNoTracking().Include(u => u.Alergias).AsNoTracking().Include(u => u.Agendamentos).ThenInclude(a => a.Vacina).AsNoTracking().ToListAsync();
        }

        public async Task<Usuario> FindUsuarioById(int id) {
            var usuarioExists = await _vacinacaoContext.Usuarios.AnyAsync(u => u.Id == id);
            if (!usuarioExists) {
                throw new ArgumentException("Usuário não encontrado");
            }
            var usuario = await _vacinacaoContext.Usuarios.AsNoTracking().Where(u => u.Id == id).Include(u => u.Alergias).AsNoTracking().Include(u => u.Agendamentos).ThenInclude(a => a.Vacina).AsNoTracking().FirstAsync();

            return usuario;
        }

        public async Task InsertUsuario(InsertUsuarioDTO usuarioDto) {
            var usuario = new Usuario(usuarioDto);
            if(await _vacinacaoContext.Usuarios.AnyAsync(u => u.Email == usuario.Email)) {
                throw new EmailJaCadastradoException("Não é possível utilizar um email já cadastrado!");
            }
            if (usuarioDto.Alergias != null && usuarioDto.Alergias.Any()) {
                usuario.Alergias = await _vacinacaoContext.Alergias.Where(a => usuarioDto.Alergias!.Contains(a.Id)).ToListAsync();
            }
            usuario.IsAdmin = false;
            await _vacinacaoContext.Usuarios.AddAsync(usuario);
            await _vacinacaoContext.SaveChangesAsync();
            _vacinacaoContext.Usuarios.Entry(usuario).State = EntityState.Detached;
        }

        public async Task InsertUsuarioAdmin(InsertUsuarioDTO usuarioDto) {
            var usuario = new Usuario(usuarioDto);
            if (await _vacinacaoContext.Usuarios.AnyAsync(u => u.Email == usuario.Email)) {
                throw new EmailJaCadastradoException("Não é possível utilizar um email já cadastrado!");
            }
            if (usuarioDto.Alergias != null && usuarioDto.Alergias.Any()) {
                usuario.Alergias = await _vacinacaoContext.Alergias.Where(a => usuarioDto.Alergias!.Contains(a.Id)).ToListAsync();
            }
            await _vacinacaoContext.Usuarios.AddAsync(usuario);
            await _vacinacaoContext.SaveChangesAsync();
            _vacinacaoContext.Usuarios.Entry(usuario).State = EntityState.Detached;
        }

        public async Task DeleteUsuario(int id) {
            var usuario = new Usuario { Id = id };
            _vacinacaoContext.Usuarios.Attach(usuario);
            _vacinacaoContext.Usuarios.Remove(usuario);
            await _vacinacaoContext.SaveChangesAsync();
        }
    }
}
