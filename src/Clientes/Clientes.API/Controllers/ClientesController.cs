using AutoMapper;
using Clientes.Domain.Dto;
using Clientes.Domain.Interfaces;
using Clientes.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Clientes.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteReadRepository _clienteReadRepository;
        private readonly IClienteWriteRepository _clienteWriteRepository;
        private readonly IMapper _mapper;

        public ClientesController(IClienteReadRepository clienteReadRepository, IClienteWriteRepository clienteWriteRepository, IMapper mapper)
        {
            _clienteReadRepository = clienteReadRepository;
            _clienteWriteRepository = clienteWriteRepository;
            _mapper = mapper;
        }

        [HttpGet("clientes")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<List<ClienteDto>>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Listar os clientes")]
        public ActionResult Get()
        {
            var records = _clienteReadRepository.GetAll().ToArray();

            if (records == null || records.Length == 0)
                throw new Exception("Nenhum registro foi encontrado.");

            return Ok(new BaseResponse<List<ClienteDto>>(_mapper.Map<List<ClienteDto>>(records))
            {
                Message = $"[{records.Length}] registro(s) encontrado(s)."
            });
        }

    }
}
