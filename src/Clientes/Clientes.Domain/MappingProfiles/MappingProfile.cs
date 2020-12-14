using AutoMapper;
using Clientes.Domain.Dto;
using Clientes.Domain.Entities;
using System;
using System.Globalization;

namespace Clientes.Domain.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<string, DateTime>().ConvertUsing(new DateTimeConverterForString());
            CreateMap<string, DateTime?>().ConvertUsing(new NullableDateTimeConverterForString());
            CreateMap<DateTime, string>().ConvertUsing(new StringConverterForDateTime());
            CreateMap<DateTime?, string>().ConvertUsing(new StringConverterForNullableDateTime());

            CreateMap<BaseEntity, BaseEntityDto>();
            CreateMap<BaseEntityDto, BaseEntity>();

            CreateMap<Cliente, ClienteDto>();

            CreateMap<ClienteDto, Cliente>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }

        private class DateTimeConverterForString : ITypeConverter<string, DateTime>
        {
            public DateTime Convert(string source, DateTime destination, ResolutionContext context)
            {
                destination = System.Convert.ToDateTime(source, new CultureInfo("pt-BR"));
                return destination;
            }
        }

        private class StringConverterForDateTime : ITypeConverter<DateTime, string>
        {
            string ITypeConverter<DateTime, string>.Convert(DateTime source, string destination, ResolutionContext context)
            {
                destination = source.ToString(new CultureInfo("pt-BR"));
                return destination;
            }
        }

        private class NullableDateTimeConverterForString : ITypeConverter<string, DateTime?>
        {
            public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
            {
                if (!string.IsNullOrEmpty(source))
                {
                    destination = System.Convert.ToDateTime(source, new CultureInfo("pt-BR"));
                    return destination;
                }

                return null;
            }
        }

        private class StringConverterForNullableDateTime : ITypeConverter<DateTime?, string>
        {
            public string Convert(DateTime? source, string destination, ResolutionContext context)
            {
                if (source.HasValue)
                {
                    destination = source.Value.ToString(new CultureInfo("pt-BR"));
                    return destination;
                }

                return "";
            }
        }
    }
}