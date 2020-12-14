using System;
using System.ComponentModel.DataAnnotations;

namespace Clientes.Domain.Dto
{
    public class BaseEntityDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Data Criação")]
        public string DataCriacao { get; protected set; }


        [Display(Name = "Data Alteração")]
        public string DataAlteracao { get; protected set; }

        /// <summary>
        /// Returna se esse registro é novo ou não
        /// </summary>
        /// <returns></returns>
        public bool IsNew()
        {
            if (Id == null || string.IsNullOrEmpty(Id.ToString()))
                return true;

            return false;
        }
    }
}
