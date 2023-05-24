using BCrypt.Net;
using System.Text.Json.Serialization;
using vacinacao_backend.Models.DTOs;

namespace vacinacao_backend.Models
{
    public class Usuario {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateOnly DataNascimento { get; set; }
        public char Sexo { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Setor { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Senha { get; set; }
        public List<Agenda>? Agendamentos { get; set; }
        public List<Alergia>? Alergias { get; set; }

        public Usuario() { }

        public Usuario(InsertUsuarioDTO dto) {
            Nome = dto.Nome;
            DataNascimento = dto.DataNascimento;
            Sexo = dto.Sexo;
            Logradouro = dto.Logradouro;
            Numero = dto.Numero;
            Setor = dto.Setor;
            Cidade = dto.Cidade;
            UF = dto.UF;
            IsAdmin = dto.IsAdmin;
            Email = dto.Email;
            Senha = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.Senha);
        }

        public int GetIdade() {
            int idade = DateTime.Now.Year - DataNascimento.Year;
            var dataAtual = DateTime.Now;

            if(DataNascimento.Month > dataAtual.Month || (DataNascimento.Month == dataAtual.Month && DataNascimento.Day > dataAtual.Day)) {
                idade--;
            }

            return idade;
        }

        public bool IsUsuarioMaiorDeIdade() {
            return GetIdade() >= 18;
        }
    }
}
