using Microsoft.EntityFrameworkCore;
using vacinacao_backend.Models;
using vacinacao_backend.Repositories;

namespace vacinacao_backend.Services {
    public class VacinaService {

        private readonly VacinacaoContext _vacinacaoContext;

        public VacinaService(VacinacaoContext vacinacaoContext) {
            _vacinacaoContext = vacinacaoContext;
        }

        public async Task<List<Vacina>> FindAllVacinas() {
            return await _vacinacaoContext.Vacinas.AsNoTracking().ToListAsync();
        }

        public async Task InsertVacina(Vacina vacina) {
            await _vacinacaoContext.Vacinas.AddAsync(vacina);
            await _vacinacaoContext.SaveChangesAsync();
            _vacinacaoContext.Vacinas.Entry(vacina).State = EntityState.Detached;
        }

        public async Task DeleteVacina(int id) {
            var vacina = new Vacina { Id = id };
            _vacinacaoContext.Vacinas.Attach(vacina);
            _vacinacaoContext.Vacinas.Remove(vacina);
            await _vacinacaoContext.SaveChangesAsync();
        }
    }
}
