using Clientes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clientes.Infrastructure.Data.EntityConfig
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
            //O nome da tabela no banco de dados
            builder.ToTable("Cliente");

            //Definir a chave primaria
            builder.HasKey(p => p.Id);

            //Essa coluna deve ser do tipo "nvarchar" e o tamanho máximo de 100 caracteres e o preenchimento é obrigatório
            builder.Property(p => p.Nome)
                .HasColumnType("nvarchar(100)")
                .IsRequired()
                .HasMaxLength(100);

            //O preenchimento dessa coluna é obrigatório
            builder.Property(p => p.Idade)
                .IsRequired();

            builder.Property(p => p.DataCriacao)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(p => p.DataAlteracao)
                .HasColumnType("datetime")
                .IsRequired(false);
        }
    }
}
