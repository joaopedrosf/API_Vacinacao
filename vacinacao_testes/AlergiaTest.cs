using vacinacao_backend.Models;

namespace vacinacao_testes {
    public class AlergiaTest {

        [Fact]
        public async Task InsertAlergiaTest() {
            var alergiaService = ServiceFactory.CreateAlergiaService();

            var alergia = new Alergia { Nome = "Alergia teste" };
            await alergiaService.InsertAlergia(alergia);

            var alergias = await alergiaService.FindAllAlergias();

            Assert.Single(alergias);
            Assert.Equal("Alergia teste", alergias[0].Nome);
        }

        [Fact]
        public async Task DeleteAlergiaTest() {
            var alergiaService = ServiceFactory.CreateAlergiaService();

            var alergia = new Alergia { Nome = "Alergia teste" };
            await alergiaService.InsertAlergia(alergia);
            var alergias = await alergiaService.FindAllAlergias();

            await alergiaService.DeleteAlergia(alergias[0].Id);

            alergias = await alergiaService.FindAllAlergias();

            Assert.Empty(alergias);
        }
    }
}
