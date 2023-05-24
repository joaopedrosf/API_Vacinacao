using Microsoft.EntityFrameworkCore;
using vacinacao_backend.Models;
using vacinacao_backend.Repositories;

namespace vacinacao_backend.Services {
    public class AlergiaService {

        private readonly VacinacaoContext _vacinacaoContext;

        public AlergiaService(VacinacaoContext vacinacaoContext) {
            _vacinacaoContext = vacinacaoContext;
        }

        public async Task<List<Alergia>> FindAllAlergias() {
            return await _vacinacaoContext.Alergias.AsNoTracking().ToListAsync();
        }

        public async Task InsertAlergia(Alergia alergia) {
            await _vacinacaoContext.Alergias.AddAsync(alergia);
            await _vacinacaoContext.SaveChangesAsync();
            _vacinacaoContext.Entry(alergia).State = EntityState.Detached;
        }

        public async Task DeleteAlergia(int id) {
            var alergia = new Alergia { Id = id };
            _vacinacaoContext.Alergias.Attach(alergia);
            _vacinacaoContext.Alergias.Remove(alergia);
            await _vacinacaoContext.SaveChangesAsync();
        }
    }
}
