using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clientes.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo [Nome] é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo [Nome] não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo [Idade] é obrigatório.")]
        [Range(1, 5000, ErrorMessage = "O campo [Idade] não pode ser menor que 1 e maior que 5000.")]
        public int Idade { get; set; }

        public Cliente() { }

        public Cliente(string nome, int idade)
        {
            // Validar os dados do registro.
            var validationErrors = Valid(nome, idade);
            if (validationErrors != null && validationErrors.Length > 0)
                throw new Exception(string.Join(";", validationErrors));

            Id = Guid.NewGuid();
            Nome = nome;
            Idade = idade;
        }

        // Para controlar quais campos podem ser atualizados.
        public void Update(string nome, int idade)
        {
            if (IsNew())
                throw new Exception("O campo [Id] inválido.");

            // Validar os dados do registro.
            var validationErrors = Valid(nome, idade);
            if (validationErrors != null && validationErrors.Length > 0)
                throw new Exception(string.Join(";", validationErrors));

            Nome = nome;
            Idade = idade;
        }

        public string[] Valid()
        {
            return Valid(Nome, Idade);
        }

        public string[] Valid(string nome, int idade)
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(nome))
                result.Add("O campo [Nome] é obrigatório.");

            else if (nome.Length > 100)
                result.Add("O campo [Nome] não pode ter mais de 100 caracteres.");

            if (idade <= 0)
                result.Add("O campo [Idade] é obrigatório.");

            return result.ToArray();
        }
    }
}
