using System;

namespace Clientes.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }

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
