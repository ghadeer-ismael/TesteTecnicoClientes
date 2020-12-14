using System;
using System.ComponentModel.DataAnnotations;

namespace Clientes.Domain.Dto
{
    public class ClienteDto : BaseEntityDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O campo [Nome] é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo [Nome] não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }

        [Range(1, 5000, ErrorMessage = "O campo [Idade] não pode ser menor que 1 e maior que 5000.")]
        public int Idade { get; set; }
    }
}
