using AutoMapper;
using Clientes.Domain.Dto;
using Clientes.Domain.Entities;
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

        [HttpGet("clientes/{id}")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Listar um cliente")]
        public ActionResult Get(Guid id)
        {
            var record = _clienteReadRepository.GetById(id);

            if (record == null)
                throw new Exception("Não encontrado.");

            return Ok(new BaseResponse<ClienteDto>(_mapper.Map<ClienteDto>(record)));
        }

        [HttpPost("clientes")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Cadastrar novo cliente")]
        public ActionResult Post([FromBody] ClienteDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new Cliente(model.Nome, model.Idade);
            _clienteWriteRepository.Add(entity);

            var record = _clienteReadRepository.GetById(entity.Id);

            if (record == null)
                throw new Exception("Falha ao salvar o registro.");

            return Ok(new BaseResponse<ClienteDto>(_mapper.Map<ClienteDto>(record))
            {
                Message = "Registro salvo com sucesso."
            });
        }

        [HttpPut("clientes")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Atualizar um cliente")]
        public ActionResult Put([FromBody] ClienteDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Localizar o registro
            var entityDb = _clienteReadRepository.GetById(model.Id);
            if (entityDb == null)
                throw new Exception("Não encontrado.");

            //Atualizar apenas o Nome e a Idade.
            entityDb.Update(model.Nome, model.Idade);

            _clienteWriteRepository.Update(entityDb);

            var record = _clienteReadRepository.GetById(entityDb.Id);

            if (record == null)
                throw new Exception("Falha ao atualizar o registro.");

            return Ok(new BaseResponse<ClienteDto>(_mapper.Map<ClienteDto>(record))
            {
                Message = "Registro atualizado com sucesso."
            });
        }

        [HttpDelete("clientes/{id}")]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(BaseResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(Summary = "Excluir um cliente")]
        public ActionResult Delete(Guid id)
        {
            var recordAntes = _clienteReadRepository.GetById(id);
            if (recordAntes == null)
                throw new Exception("Não encontrado.");

            _clienteWriteRepository.Delete(new Cliente() { Id = id });

            var recordDepois = _clienteReadRepository.GetById(id);

            if (recordDepois != null)
                throw new Exception("Falha ao excluir o registro.");

            return Ok(new BaseResponse()
            {
                Message = "Registro excluído com sucesso."
            });
        }
    }
}
