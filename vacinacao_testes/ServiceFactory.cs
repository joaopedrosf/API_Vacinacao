using Microsoft.EntityFrameworkCore;
using vacinacao_backend.Hubs;
using vacinacao_backend.Repositories;
using vacinacao_backend.Services;

namespace vacinacao_testes {
    public static class ServiceFactory {

        public static VacinacaoContext CreateContext() {
            var optionsBuilder = new DbContextOptionsBuilder<VacinacaoContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new VacinacaoContext(optionsBuilder.Options);
            return context;
        }

        public static UsuarioService CreateUsuarioService(VacinacaoContext context = null) {
            if(context == null) {
                return new UsuarioService(CreateContext());
            }
            return new UsuarioService(context);
        }

        public static AlergiaService CreateAlergiaService(VacinacaoContext context = null) {
            if(context == null) {
                return new AlergiaService(CreateContext());
            }
            return new AlergiaService(context);
        }

        public static AgendaService CreateAgendaService(VacinacaoContext context = null) {
            if(context == null) {
                return new AgendaService(CreateContext(), new VacinacaoHub());
            }
            return new AgendaService(context, new VacinacaoHub());
        }

        public static VacinaService CreateVacinaService(VacinacaoContext context = null) {
            if(context == null) {
                return new VacinaService(CreateContext());
            }
            return new VacinaService(context);
        }
    }
}
